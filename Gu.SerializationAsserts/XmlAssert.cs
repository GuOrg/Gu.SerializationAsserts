namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;

    public static class XmlAssert
    {
        /// <summary>
        /// Parses the xml and compares expected to actual.
        /// </summary>
        /// <param name="expected">The expected xml</param>
        /// <param name="actual">The actual xml</param>
        /// <param name="options">How to compare the xml</param>
        public static void Equal(string expected, string actual, XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            var expectedXml = ParseDocument(expected, nameof(expected), options);
            var actualXml = ParseDocument(actual, nameof(actual), options);
            Equal(expectedXml, actualXml, options);
        }

        private static void Equal(XDocumentAndSource expected, XDocumentAndSource actual, XmlAssertOptions options)
        {
            if (!options.HasFlag(XmlAssertOptions.IgnoreDeclaration))
            {
                if (!FieldsEqualsComparer<XDeclaration>.Default.Equals(expected.Document.Declaration, actual.Document.Declaration))
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }
            }

            Equal(expected.Element, actual.Element, options);
        }

        private static void Equal(XElementAndSource expected, XElementAndSource actual, XmlAssertOptions options)
        {
            if (!XNameComparer.GetFor(options).Equals(expected.Element.Name, actual.Element.Name))
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            if (expected.Attributes.Count != actual.Attributes.Count)
            {
                var message = $"  Number of attributes does not macth for {expected.Element.Name.LocalName}\r\n" +
                              $"  Expected: {expected.Attributes.Count}\r\n" +
                              $"  But was:  {actual.Attributes.Count}\r\n" +
                              CreateMessage( expected, actual);
                throw new AssertException(message);
            }

            Equal(expected.Attributes, actual.Attributes, options);

            if (expected.Elements.Count == 0 && actual.Elements.Count == 0)
            {
                if (expected.Element.Value != actual.Element.Value)
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }

                return;
            }

            if (expected.Elements.Count != actual.Elements.Count)
            {
                var message = $"  Number of elements does not macth for {expected.Element.Name}\r\n" +
                              $"  Expected: {expected.Elements.Count}\r\n" +
                              $"  But was:  {actual.Elements.Count}\r\n" +
                              CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            Equal(expected.Elements, actual.Elements, options);
        }

        private static void Equal(IReadOnlyList<XAttributeAndSource> expecteds, IReadOnlyList<XAttributeAndSource> actuals, XmlAssertOptions options)
        {
            for (int i = 0; i < Math.Max(expecteds.Count, actuals.Count); i++)
            {
                var expected = expecteds.ElementAtOrDefault(i);
                var actual = actuals.ElementAtOrDefault(i);
                Equal(expected, actual, options);
            }
        }

        private static void Equal(XAttributeAndSource expected, XAttributeAndSource actual, XmlAssertOptions options)
        {
            if (!XNameComparer.GetFor(options).Equals(expected.Attribute.Name, actual.Attribute.Name) ||
                expected.Attribute.Value != actual.Attribute.Value)
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }
        }

        private static void Equal(IReadOnlyList<XElementAndSource> expecteds, IReadOnlyList<XElementAndSource> actuals, XmlAssertOptions options)
        {
            for (int i = 0; i < Math.Max(expecteds.Count, actuals.Count); i++)
            {
                var expected = expecteds.ElementAtOrDefault(i);
                var actual = actuals.ElementAtOrDefault(i);
                Equal(expected, actual, options);
            }
        }

        private static XDocumentAndSource ParseDocument(string xml, string parameterName, XmlAssertOptions options)
        {
            try
            {
                return new XDocumentAndSource(xml, XDocument.Parse(xml, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo), options);
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException($"{parameterName} is not valid xml.", e);
            }
        }

        private static string CreateMessage(IXAndSource expected, IXAndSource actual)
        {
            var expectedLine = expected.SourceXml.Line(expected.LineNumber).Trim();
            var actualLine = actual.SourceXml.Line(actual.LineNumber).Trim();
            var index = expectedLine.FirstDiff(actualLine);

            using (var writer = new StringWriter())
            {
                if (expectedLine.Length != actualLine.Length)
                {
                    writer.WriteLine($"  Expected string length {expected.SourceXml.Length} but was {actual.SourceXml.Length}.");
                }
                else
                {
                    writer.WriteLine($"  String lengths are both {expected.SourceXml.Length}.");
                }

                writer.WriteLine($"  Strings differ at line {expected.LineNumber} index {index}.");
                writer.WriteLine($"  Expected: {expected.LineNumber}| {expectedLine}");
                writer.WriteLine($"  But was:  {actual.LineNumber}| {actualLine}");
                writer.Write($"  {new string('-', index + 13)}^");
                return writer.ToString();
            }
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new AssertException($"{x}, {y}");
        }
    }
}
