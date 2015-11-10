namespace Gu.SerializationAsserts
{
    using System.Xml;
    using System.Xml.Linq;

    internal class XAttributeAndSource
    {
        public XAttributeAndSource(string sourceXml, XAttribute attribute)
        {
            Ensure.NotNull(attribute, nameof(attribute));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            this.SourceXml = sourceXml;
            this.Attribute = attribute;
        }

        public string SourceXml { get; }

        public XAttribute Attribute { get; }

        public int LineNumber => (this.Attribute as IXmlLineInfo)?.LineNumber ?? 0;
    }
}