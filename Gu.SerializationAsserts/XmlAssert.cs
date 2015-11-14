namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public static class XmlAssert
    {
        private static readonly XName[] EmptyNames = new XName[0];

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
            Equal(expectedXml, actualXml, new XElementComparer(options), new XAttributeComparer(options), options);
        }

        public static void Equal(
            string expected,
            string actual,
            IEqualityComparer<XElement> elementComparer,
            IEqualityComparer<XAttribute> attributeComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            var expectedXml = ParseDocument(expected, nameof(expected), options);
            var actualXml = ParseDocument(actual, nameof(actual), options);
            Equal(expectedXml, actualXml, elementComparer, attributeComparer, options);
        }

        private static void Equal(XDocumentAndSource expected, XDocumentAndSource actual, IEqualityComparer<XElement> elementComparer, IEqualityComparer<XAttribute> attributeComparer, XmlAssertOptions options)
        {
            if (!options.HasFlag(XmlAssertOptions.IgnoreDeclaration))
            {
                if (!FieldsEqualsComparer<XDeclaration>.Default.Equals(expected.Document.Declaration, actual.Document.Declaration))
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }
            }

            Equal(expected.Element, actual.Element, elementComparer, attributeComparer, options);
        }

        private static void Equal(XElementAndSource expected, XElementAndSource actual, IEqualityComparer<XElement> elementComparer, IEqualityComparer<XAttribute> attributeComparer, XmlAssertOptions options)
        {
            var nameComparer = XNameComparer.GetFor(options);

            CheckAttributeOrder(expected, actual, options);

            // ReSharper disable once InvokeAsExtensionMethod
            var childAttributeNames = Enumerable.Concat(
                expected?.Attributes.Select(x => x.Attribute.Name) ?? EmptyNames,
                actual?.Attributes.Select(x => x.Attribute.Name) ?? EmptyNames)
                                              .Distinct(nameComparer);
            foreach (var name in childAttributeNames)
            {
                var expectedAttribute = expected?.Attributes.SingleOrDefault(x => nameComparer.Equals(x.Attribute.Name, name));
                var actualAttribute = actual?.Attributes.SingleOrDefault(x => nameComparer.Equals(x.Attribute.Name, name));
                if (XAttributeComparer.GetFor(options).Equals(expectedAttribute?.Attribute, actualAttribute?.Attribute))
                {
                    continue;
                }

                if (!attributeComparer.Equals(expectedAttribute?.Attribute, actualAttribute?.Attribute))
                {
                    var message = CreateMessage(expectedAttribute, actualAttribute);
                    throw new AssertException(message);
                }
            }

            if ((expected?.Elements.Count ?? 0) == 0 && (actual?.Elements.Count ?? 0) == 0)
            {
                if (XElementComparer.GetFor(options).Equals(expected?.Element, actual?.Element))
                {
                    return;
                }

                if (!elementComparer.Equals(expected?.Element, actual?.Element))
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }

                return;
            }

            if (!nameComparer.Equals(expected?.Element.Name, actual?.Element.Name))
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            CheckElementOrder(expected, actual, options);

            // ReSharper disable once InvokeAsExtensionMethod
            var childElementNames = Enumerable.Concat(
                expected?.Elements.Select(x => x.Element.Name) ?? EmptyNames,
                actual?.Elements.Select(x => x.Element.Name) ?? EmptyNames)
                                              .Distinct(nameComparer);

            foreach (var name in childElementNames)
            {
                var expectedChild = expected?.Elements.SingleOrDefault(x => nameComparer.Equals(x.Element.Name, name));
                var actualChild = actual?.Elements.SingleOrDefault(x => nameComparer.Equals(x.Element.Name, name));
                Equal(expectedChild, actualChild, elementComparer, attributeComparer, options);
            }
        }

        private static void CheckAttributeOrder(XElementAndSource expected, XElementAndSource actual, XmlAssertOptions options)
        {
            if (options.HasFlag(XmlAssertOptions.IgnoreAttributeOrder))
            {
                return;
            }

            var nameComparer = XNameComparer.GetFor(options);
            int index = -1;
            foreach (var attribute in expected.Attributes)
            {
                var indexOf = actual.Attributes.IndexOf(attribute, x => x.Attribute.Name, nameComparer);
                if (indexOf < 0)
                {
                    continue;
                }

                if (index > indexOf)
                {
                    var message = CreateMessage(attribute, actual.Attributes[indexOf], "  The order of atributes is incorrect.");
                    throw new AssertException(message);
                }

                index = indexOf;
            }
        }

        private static void CheckElementOrder(XElementAndSource expected, XElementAndSource actual, XmlAssertOptions options)
        {
            if (options.HasFlag(XmlAssertOptions.IgnoreElementOrder))
            {
                return;
            }

            var nameComparer = XNameComparer.GetFor(options);
            int index = -1;
            foreach (var element in expected.Elements)
            {
                var indexOf = actual.Elements.IndexOf(element, x => x.Element.Name, nameComparer);
                if (indexOf < 0)
                {
                    continue;
                }

                if (index > indexOf)
                {
                    var message = CreateMessage(element, actual.Elements[indexOf], "  The order of elements is incorrect.");
                    throw new AssertException(message);
                }

                index = indexOf;
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
            using (var writer = new StringWriter())
            {
                writer.WriteLine("  Expected and actual xml are not equal.");
                if (subHeader != null)
                {
                    writer.Write(subHeader);
                    writer.WriteLine();
                }

                writer.WriteLine($"  Xml differ at line {lineNumber} index {index}.");
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
