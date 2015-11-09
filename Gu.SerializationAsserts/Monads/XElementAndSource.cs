namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    internal class XElementAndSource
    {
        public XElementAndSource(string sourceXml, XElement element)
        {
            Ensure.NotNull(element, nameof(element));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            SourceXml = sourceXml;
            Element = element;
            Attributes = Element.Attributes()
                                .Select(x => new XAttributeAndSource(sourceXml, x))
                                .ToArray();

            Elements = Element.Elements()
                              .Select(x => new XElementAndSource(sourceXml, x))
                              .ToArray();

        }

        public string SourceXml { get; }

        public XElement Element { get; }

        public IReadOnlyList<XAttributeAndSource> Attributes { get; }

        public IReadOnlyList<XElementAndSource> Elements { get; }

        public int LineNumber => (Element as IXmlLineInfo)?.LineNumber ?? 0;
    }
}