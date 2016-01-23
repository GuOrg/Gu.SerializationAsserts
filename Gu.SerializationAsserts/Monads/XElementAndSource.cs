namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    [DebuggerDisplay("ElementName: {Element.Name}")]
    internal class XElementAndSource : IXAndSource
    {
        private IReadOnlyList<XAttributeAndSource> allAttributes;
        private IReadOnlyList<XAttributeAndSource> attributesToCheck;
        private IReadOnlyList<XElementAndSource> allElements;
        private IReadOnlyList<XElementAndSource> elementsToCheck;

        public XElementAndSource(string sourceXml, XElement element, XmlAssertOptions options)
        {
            Ensure.NotNull(element, nameof(element));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            this.SourceXml = sourceXml;
            this.Element = element;
            this.Options = options;
        }

        public string SourceXml { get; }

        public XElement Element { get; }

        public XmlAssertOptions Options { get; }

        public bool IsEmpty => this.AllAttributes.Count == 0 && this.AllElements.Count == 0 && string.IsNullOrEmpty(this.Element.Value);

        public IReadOnlyList<XAttributeAndSource> AllAttributes => this.allAttributes ?? (this.allAttributes = this.GetAllAttributes());

        public IReadOnlyList<XAttributeAndSource> AttributesToCheck => this.attributesToCheck ?? (this.attributesToCheck = this.GetAttributesToCheck().ToList());

        public IReadOnlyList<XElementAndSource> AllElements => this.allElements ?? (this.allElements = this.GetAllElements());

        public IReadOnlyList<XElementAndSource> ElementsToCheck => this.elementsToCheck ?? (this.elementsToCheck = this.GetElementsToCheck().ToList());

        public int LineNumber => (this.Element as IXmlLineInfo)?.LineNumber ?? 0;

        private IReadOnlyList<XAttributeAndSource> GetAllAttributes()
        {
            var attributeAndSources = this.Element.Attributes()
                                          .Where(x => !(this.Options.HasFlag(XmlAssertOptions.IgnoreNamespaces) && x.IsNamespaceDeclaration))
                                          .Select(x => new XAttributeAndSource(this.SourceXml, x, this.Options));
            if (this.Options.IsSet(XmlAssertOptions.IgnoreAttributeOrder))
            {
                var nameComparer = XNameComparer.GetFor(this.Options);
                return attributeAndSources.OrderBy(x => x.Attribute.Name, nameComparer)
                                          .ToList();
            }

            return attributeAndSources.ToList();
        }

        private IEnumerable<XAttributeAndSource> GetAttributesToCheck()
        {
            foreach (var attribute in this.AllAttributes)
            {
                if (attribute.Attribute.IsNamespaceDeclaration &&
                    this.Options.IsSet(XmlAssertOptions.IgnoreNamespaces))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(attribute.Attribute.Value) &&
                    this.Options.IsSet(XmlAssertOptions.TreatEmptyAndMissingAttributesAsEqual))
                {
                    continue;
                }

                yield return attribute;
            }
        }

        private IReadOnlyList<XElementAndSource> GetAllElements()
        {
            var elementAndSources = this.Element.Elements().Select(x => new XElementAndSource(this.SourceXml, x, this.Options));
            if (this.Options.IsSet(XmlAssertOptions.IgnoreElementOrder))
            {
                var nameComparer = XNameComparer.GetFor(this.Options);
                return elementAndSources.OrderBy(x => x.Element.Name, nameComparer)
                    .ToList();
            }

            return elementAndSources.ToList();
        }

        private IEnumerable<XElementAndSource> GetElementsToCheck()
        {
            foreach (var element in this.AllElements)
            {
                if (element.IsEmpty &&
                    this.Options.IsSet(XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual))
                {
                    continue;
                }

                yield return element;
            }
        }
    }
}