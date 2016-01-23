namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
            if (!options.IsSet(XmlAssertOptions.IgnoreDeclaration))
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
            CheckAttributes(expected, actual, customAttributeComparer, options);

            if ((expected?.AllElements.Count ?? 0) == 0 && (actual?.AllElements.Count ?? 0) == 0)
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

            var nameComparer = XNameComparer.GetFor(options);
            if (!nameComparer.Equals(expected?.Element.Name, actual?.Element.Name))
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            if (!options.IsSet(XmlAssertOptions.IgnoreElementOrder))
            {
                var expectedElements = expected?.AllElements;
                var actualElements = actual?.AllElements;
                CheckOrder(expectedElements,
                           actualElements,
                           x => x.Element.Name,
                           "  The order of elements is incorrect.",
                           options);
            }

            var expectedElementsToCheck = expected?.ElementsToCheck;
            var actualElementsToCheck = actual?.ElementsToCheck;
            for (int i = 0; i < Math.Max(expectedElementsToCheck?.Count ?? 0, actualElementsToCheck?.Count ?? 0); i++)
            {
                var expectedChild = expectedElementsToCheck.ElementAtOrDefault(i);
                var actualChild = actualElementsToCheck.ElementAtOrDefault(i);
                Equal(expectedChild, actualChild, customElementComparer, customAttributeComparer, options);
            }
        }

        private static void CheckAttributes(
            XElementAndSource expectedElement,
            XElementAndSource actualElement,
            IEqualityComparer<XAttribute> customAttributeComparer,
            XmlAssertOptions options)
        {
            if (!options.IsSet(XmlAssertOptions.IgnoreAttributeOrder))
            {
                var expectedAttributes = expectedElement?.AllAttributes;
                var actualAttributes = actualElement?.AllAttributes;
                CheckOrder(expectedAttributes,
                           actualAttributes,
                           x => x.Attribute.Name,
                           "  The order of attributes is incorrect.",
                           options);
            }

            var expectedAttributesToCheck = expectedElement?.AttributesToCheck;
            var actualAttributesToCheck = actualElement?.AttributesToCheck;

            var defaultAttributeComparer = XAttributeComparer.GetFor(options);
            for (int i = 0; i < Math.Max(expectedAttributesToCheck?.Count ?? 0, actualAttributesToCheck?.Count ?? 0); i++)
            {
                var expectedAttribute = expectedAttributesToCheck.ElementAtOrDefault(i);
                var actualAttribute = actualAttributesToCheck.ElementAtOrDefault(i);

                if (defaultAttributeComparer.Equals(expectedAttribute?.Attribute, actualAttribute?.Attribute))
                {
                    continue;
                }

                if (customAttributeComparer?.Equals(expectedAttribute?.Attribute, actualAttribute?.Attribute) == true)
                {
                    continue;
                }

                var message = expectedAttribute == null || actualAttribute == null
                    ? CreateMessage(expectedElement, actualElement)
                    : CreateMessage(expectedAttribute, actualAttribute);
                throw new AssertException(message);
            }
        }

        private static void CheckOrder<T>(
            IReadOnlyList<T> expecteds,
            IReadOnlyList<T> actuals,
            Func<T, XName> nameGetter,
            string errorMessage,
            XmlAssertOptions options)
            where T : IXAndSource
        {
            if (expecteds == null || actuals == null)
            {
                return;
            }

            var nameComparer = XNameComparer.GetFor(options);
            var actualIndex = 0;
            foreach (var expected in expecteds)
            {
                var indexOf = actuals.IndexOf(expected, nameGetter, actualIndex, nameComparer);
                if (indexOf < 0)
                {
                    continue;
                }

                if (actualIndex > indexOf)
                {
                    var actual = actuals[indexOf];
                    var message = CreateMessage(expected, actual, errorMessage);
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
            if (expectedLine == actualLine ||
                actual == null ||
                expected == null)
            {
                index = -1;
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
                else if (expected != null && actual != null)
                {
                    writer.WriteLine($"  Line {expected?.LineNumber} in expected is found at line {actual?.LineNumber} in actual.");
                }
                else if (expected != null)
                {
                    writer.WriteLine($"  Element at line {expected.LineNumber} in expected not found in actual.");
                }
                else if (actual != null)
                {
                    writer.WriteLine($"  Element at line {actual.LineNumber} in actual not found in expected.");
                }

                if (expected == null)
                {
                    writer.WriteLine($"  Expected:  | No element");
                }
                else
                {
                    writer.WriteLine($"  Expected: {expected?.LineNumber}| {expectedLine}");
                }

                if (actual != null)
                {
                    writer.WriteLine($"  But was:  {actual.LineNumber.ToString()}| {actualLine}");
                }
                else
                {
                    writer.WriteLine($"  But was:  {new string('?', expected.LineNumber.ToString().Length)}| Missing");
                }

                if (index >= 0)
                {
                    writer.Write($"  {new string('-', index + 13)}^");
                }
                else
                {
                    writer.Write($"  {new string('-', 10)}^");
                }

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
