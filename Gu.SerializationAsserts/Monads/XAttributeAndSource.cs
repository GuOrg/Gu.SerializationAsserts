namespace Gu.SerializationAsserts
{
    using System.Diagnostics;
    using System.Xml;
    using System.Xml.Linq;

    [DebuggerDisplay("AttributeName: {Attribute.Name}, Value: {Attribute.Value}")]
    internal class XAttributeAndSource
    {
        public XAttributeAndSource(string sourceXml, XAttribute attribute, XmlAssertOptions options)
        {
            Ensure.NotNull(attribute, nameof(attribute));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            this.SourceXml = sourceXml;
            this.Attribute = attribute;
            this.Options = options;
        }

        public string SourceXml { get; }

        public XAttribute Attribute { get; }

        public XmlAssertOptions Options { get; }

        public int LineNumber => (this.Attribute as IXmlLineInfo)?.LineNumber ?? 0;
    }
}