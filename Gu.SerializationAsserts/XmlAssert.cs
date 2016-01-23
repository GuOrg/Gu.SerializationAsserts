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
            Equal(expected, actual, null, null, options);
        }

        /// <summary>
        /// Parses the xml and compares expected to actual.
        /// </summary>
        /// <param name="expected">The expected xml</param>
        /// <param name="actual">The actual xml</param>
        /// <param name="elementComparer">Additional comparer used for comparing leaf elements</param>
        /// <param name="options">How to compare the xml</param>
        public static void Equal(
            string expected,
            string actual,
            IEqualityComparer<XElement> elementComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            Equal(expected, actual, elementComparer, null, options);
        }

        /// <summary>
        /// Parses the xml and compares expected to actual.
        /// </summary>
        /// <param name="expected">The expected xml</param>
        /// <param name="actual">The actual xml</param>
        /// <param name="attributeComparer">Additional comparer used for comparing attributes</param>
        /// <param name="options">How to compare the xml</param>
        public static void Equal(
            string expected,
            string actual,
            IEqualityComparer<XAttribute> attributeComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            Equal(expected, actual, null, attributeComparer, options);
        }

        /// <summary>
        /// Parses the xml and compares expected to actual.
        /// </summary>
        /// <param name="expected">The expected xml</param>
        /// <param name="actual">The actual xml</param>
        /// <param name="elementComparer">Additional comparer used for comparing leaf elements</param>
        /// <param name="attributeComparer">Additional comparer used for comparing attributes</param>
        /// <param name="options">How to compare the xml</param>
        public static void Equal(
            string expected,
            string actual,
            IEqualityComparer<XElement> elementComparer,
            IEqualityComparer<XAttribute> attributeComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            var expectedXml = ParseDocument(expected, nameof(expected), options);

            // we want to parse first to assert that it is valid xml
            if (expected == actual)
            {
                return;
            }

            var actualXml = ParseDocument(actual, nameof(actual), options);
            Equal(expectedXml, actualXml, elementComparer, attributeComparer, options);
        }

        private static void Equal(
            XDocumentAndSource expected,
            XDocumentAndSource actual,
            IEqualityComparer<XElement> elementComparer,
            IEqualityComparer<XAttribute> attributeComparer,
            XmlAssertOptions options)
        {
            if (!options.HasFlag(XmlAssertOptions.IgnoreDeclaration))
            {
                if (!XDeclarationComparer.Default.Equals(expected.Document.Declaration, actual.Document.Declaration))
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }
            }

            Equal(expected.Element, actual.Element, elementComparer, attributeComparer, options);
        }

        private static void Equal(
            XElementAndSource expected,
            XElementAndSource actual,
            IEqualityComparer<XElement> customElementComparer,
            IEqualityComparer<XAttribute> customAttributeComparer,
            XmlAssertOptions options)
        {
            var nameComparer = XNameComparer.GetFor(options);
            CheckAttributeOrder(expected, actual, options);
            var defaultAttributeComparer = XAttributeComparer.GetFor(options);
            for (int i = 0; i < Math.Max(expected?.Attributes.Count ?? 0, actual?.Attributes.Count ?? 0); i++)
            {
                var expectedAttribute = expected?.Attributes.ElementAtOrDefault(i);
                var actualAttribute = actual?.Attributes.ElementAtOrDefault(i);

                if (defaultAttributeComparer.Equals(expectedAttribute?.Attribute, actualAttribute?.Attribute))
                {
                    continue;
                }

                if (customAttributeComparer?.Equals(expectedAttribute?.Attribute, actualAttribute?.Attribute) == true)
                {
                    continue;
                }

                var message = expectedAttribute == null || actualAttribute == null
                    ? CreateMessage(expected, actual)
                    : CreateMessage(expectedAttribute, actualAttribute);
                throw new AssertException(message);
            }

            if ((expected?.Elements.Count ?? 0) == 0 && (actual?.Elements.Count ?? 0) == 0)
            {
                if (XElementComparer.GetFor(options).Equals(expected?.Element, actual?.Element))
                {
                    return;
                }

                if (customElementComparer?.Equals(expected?.Element, actual?.Element) == true)
                {
                    return;
                }

                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            if (!nameComparer.Equals(expected?.Element.Name, actual?.Element.Name))
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            CheckElementOrder(expected, actual, options);

            for (int i = 0; i < Math.Max(expected?.Elements.Count ?? 0, actual?.Elements.Count ?? 0); i++)
            {
                var expectedChild = expected?.Elements.ElementAtOrDefault(i);
                var actualChild = actual?.Elements.ElementAtOrDefault(i);
                Equal(expectedChild, actualChild, customElementComparer, customAttributeComparer, options);
            }
        }

        private static void CheckAttributeOrder(XElementAndSource expected, XElementAndSource actual, XmlAssertOptions options)
        {
            if (expected == null || actual == null)
            {
                return;
            }

            if (options.HasFlag(XmlAssertOptions.IgnoreAttributeOrder))
            {
                return;
            }

            var nameComparer = XNameComparer.GetFor(options);
            var actualIndex = 0;
            foreach (var attribute in expected.Attributes)
            {
                var indexOf = actual.Attributes.IndexOf(attribute, x => x.Attribute.Name, actualIndex, nameComparer);
                if (indexOf < 0)
                {
                    continue;
                }

                if (actualIndex > indexOf)
                {
                    var message = CreateMessage(attribute, actual.Attributes[indexOf], "  The order of attributes is incorrect.");
                    throw new AssertException(message);
                }

                actualIndex = indexOf;
            }
        }

        private static void CheckElementOrder(XElementAndSource expected, XElementAndSource actual, XmlAssertOptions options)
        {
            if (options.HasFlag(XmlAssertOptions.IgnoreElementOrder) ||
                expected.Elements.Count == 0 ||
                actual.Elements.Count == 0)
            {
                return;
            }

            var nameComparer = XNameComparer.GetFor(options);
            var actualIndex = 0;
            foreach (var expectedElement in expected.Elements)
            {
                var indexOf = actual.Elements.IndexOf(expectedElement, x => x.Element.Name, actualIndex, nameComparer);
                if (indexOf < 0)
                {
                    continue;
                }

                if (indexOf < actualIndex)
                {
                    var message = CreateMessage(expectedElement, actual.Elements[indexOf], "  The order of elements is incorrect.");
                    throw new AssertException(message);
                }

                actualIndex = indexOf;
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

        private static string CreateMessage(IXAndSource expected, IXAndSource actual, string subHeader = null)
        {
            var expectedLine = expected?.SourceXml.Line(expected.LineNumber).Trim();
            var actualLine = actual?.SourceXml.Line(actual.LineNumber).Trim();
            var index = expectedLine.FirstDiff(actualLine);
            var lineNumber = expected?.LineNumber ?? actual.LineNumber;
            if (expected?.LineNumber != actual?.LineNumber &&
                expectedLine == actualLine)
            {
                index = -(lineNumber.ToString().Length + 2);
            }

            using (var writer = new StringWriter())
            {
                if (subHeader != null)
                {
                    writer.Write(subHeader);
                    writer.WriteLine();
                }

                if (index >= 0)
                {
                    writer.WriteLine($"  Xml differ at line {lineNumber} index {index}.");
                }
                else
                {
                    writer.WriteLine($"  Line {expected?.LineNumber} in expected is found at line {actual?.LineNumber} in actual.");
                }

                writer.WriteLine($"  Expected: {expected?.LineNumber}| {expectedLine}");
                writer.WriteLine($"  But was:  {actual?.LineNumber.ToString() ?? new string('?', expected.LineNumber.ToString().Length)}| {actualLine ?? "Missing"}");
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
