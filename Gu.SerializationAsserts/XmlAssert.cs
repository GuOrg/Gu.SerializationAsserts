namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    public static class XmlAssert
    {
        public static void AreEqual(string expected, string actual)
        {
            AreEqual(ParseDocument(expected, nameof(expected)), ParseDocument(actual, nameof(actual)));
        }

        private static void AreEqual(XDocumentAndSource expected, XDocumentAndSource actual)
        {
            if (!FieldsEqualsComparer<XDeclaration>.Default.Equals(expected.Document.Declaration, actual.Document.Declaration))
            {
                var message = CreateMessage(1, expected.SourceXml, actual.SourceXml);
                throw new XmlAssertException(message);
            }

            AreEqual(expected.Element, actual.Element);
        }

        private static void AreEqual(XElementAndSource expected, XElementAndSource actual)
        {
            if (expected.Element.Name != actual.Element.Name)
            {
                var message = CreateMessage(expected.LineNumber, expected.SourceXml, actual.SourceXml);
                throw new XmlAssertException(message);
            }

            AreEqual(expected.Attributes, actual.Attributes);

            if (expected.Elements.Count == 0 && actual.Elements.Count == 0)
            {
                if (expected.Element.Value != actual.Element.Value)
                {
                    var message = CreateMessage(expected.LineNumber, expected.SourceXml, actual.SourceXml);
                    throw new XmlAssertException(message);
                }
                return;
            }

            AreEqual(expected.Elements, actual.Elements);
        }

        private static void AreEqual(IReadOnlyList<XAttributeAndSource> expecteds, IReadOnlyList<XAttributeAndSource> actuals)
        {
            for (int i = 0; i < Math.Max(expecteds.Count, actuals.Count); i++)
            {
                var expected = expecteds.ElementAtOrDefault(i);
                var actual = actuals.ElementAtOrDefault(i);
                AreEqual(expected, actual);
            }
        }

        private static void AreEqual(XAttributeAndSource expected, XAttributeAndSource actual)
        {
            if (expected.Attribute.Name != actual.Attribute.Name ||
                expected.Attribute.Value != actual.Attribute.Value)
            {
                var message = CreateMessage(expected.LineNumber, expected.SourceXml, actual.SourceXml);
                throw new XmlAssertException(message);
            }
        }

        private static void AreEqual(IReadOnlyList<XElementAndSource> expecteds, IReadOnlyList<XElementAndSource> actuals)
        {
            for (int i = 0; i < Math.Max(expecteds.Count, actuals.Count); i++)
            {
                var expected = expecteds.ElementAtOrDefault(i);
                var actual = actuals.ElementAtOrDefault(i);
                AreEqual(expected, actual);
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
    }
}
