namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    public static class XmlAssert
    {
        public static void Equal(string expected, string actual)
        {
            Equal(ParseDocument(expected, nameof(expected)), ParseDocument(actual, nameof(actual)));
        }

        private static void Equal(XDocumentAndSource expected, XDocumentAndSource actual)
        {
            if (!FieldsEqualsComparer<XDeclaration>.Default.Equals(expected.Document.Declaration, actual.Document.Declaration))
            {
                var message = CreateMessage(1, expected.SourceXml, actual.SourceXml);
                throw new XmlAssertException(message);
            }

            Equal(expected.Element, actual.Element);
        }

        private static void Equal(XElementAndSource expected, XElementAndSource actual)
        {
            if (expected.Element.Name != actual.Element.Name)
            {
                var message = CreateMessage(expected.LineNumber, expected.SourceXml, actual.SourceXml);
                throw new XmlAssertException(message);
            }

            Equal(expected.Attributes, actual.Attributes);

            if (expected.Elements.Count == 0 && actual.Elements.Count == 0)
            {
                if (expected.Element.Value != actual.Element.Value)
                {
                    var message = CreateMessage(expected.LineNumber, expected.SourceXml, actual.SourceXml);
                    throw new XmlAssertException(message);
                }

                return;
            }

            Equal(expected.Elements, actual.Elements);
        }

        private static void Equal(IReadOnlyList<XAttributeAndSource> expecteds, IReadOnlyList<XAttributeAndSource> actuals)
        {
            for (int i = 0; i < Math.Max(expecteds.Count, actuals.Count); i++)
            {
                var expected = expecteds.ElementAtOrDefault(i);
                var actual = actuals.ElementAtOrDefault(i);
                Equal(expected, actual);
            }
        }

        private static void Equal(XAttributeAndSource expected, XAttributeAndSource actual)
        {
            if (expected.Attribute.Name != actual.Attribute.Name ||
                expected.Attribute.Value != actual.Attribute.Value)
            {
                var message = CreateMessage(expected.LineNumber, expected.SourceXml, actual.SourceXml);
                throw new XmlAssertException(message);
            }
        }

        private static void Equal(IReadOnlyList<XElementAndSource> expecteds, IReadOnlyList<XElementAndSource> actuals)
        {
            for (int i = 0; i < Math.Max(expecteds.Count, actuals.Count); i++)
            {
                var expected = expecteds.ElementAtOrDefault(i);
                var actual = actuals.ElementAtOrDefault(i);
                Equal(expected, actual);
            }
        }

        private static XDocumentAndSource ParseDocument(string xml, string parameterName)
        {
            try
            {
                return new XDocumentAndSource(xml, XDocument.Parse(xml, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo));
            }
            catch (Exception e)
            {
                throw new XmlAssertException($"{parameterName} is not valid xml", e);
            }
        }

        private static string CreateMessage(int lineNumber, string expected, string actual)
        {
            var expectedLine = expected.Line(lineNumber).Trim();
            var actualLine = actual.Line(lineNumber).Trim();
            var index = expectedLine.FirstDiff(actualLine);

            using (var writer = new StringWriter())
            {
                if (expectedLine.Length != actualLine.Length)
                {
                    writer.WriteLine($"  Expected string length {expected.Length} but was {actual.Length}.");
                }
                else
                {
                    writer.WriteLine($"  String lengths are both {expected.Length}.");
                }

                writer.WriteLine($"  Strings differ at line {lineNumber} index {index}.");
                writer.WriteLine($"  Expected: {expectedLine}");
                writer.WriteLine($"  But was:  {actualLine}");
                writer.Write($"  {new string('-', index + 10)}^");
                return writer.ToString();
            }
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }
    }
}
