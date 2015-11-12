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

        public IReadOnlyList<XAttributeAndSource> Attributes => this.CreateAttributes();

        public IReadOnlyList<XElementAndSource> Elements => this.CreateElements();

        public int LineNumber => (this.Element as IXmlLineInfo)?.LineNumber ?? 0;

        private IReadOnlyList<XAttributeAndSource> CreateAttributes()
        {
            var attributeAndSources = this.Element.Attributes()
                                          .Where(x => !(this.Options.HasFlag(XmlAssertOptions.IgnoreNamespaces) && x.IsNamespaceDeclaration))
                                          .Select(x => new XAttributeAndSource(this.SourceXml, x, this.Options));
            if (this.Options.HasFlag(XmlAssertOptions.IgnoreAttributeOrder))
            {
                var nameComparer = XNameComparer.GetFor(this.Options);
                return attributeAndSources.OrderBy(x => x.Attribute.Name, nameComparer)
                                          .ToList();
            }

            return attributeAndSources.ToList();
        }

        private IReadOnlyList<XElementAndSource> CreateElements()
        {
            var elementAndSources = this.Element.Elements()
                                        .Select(x => new XElementAndSource(this.SourceXml, x, this.Options));
            if (this.Options.HasFlag(XmlAssertOptions.IgnoreElementOrder))
            {
                var nameComparer = XNameComparer.GetFor(this.Options);
                return elementAndSources.OrderBy(x => x.Element.Name, nameComparer)
                                          .ToList();
            }

            return elementAndSources.ToList();
        }
    }
}