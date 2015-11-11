namespace Gu.SerializationAsserts
{
    using System.Xml.Linq;

    internal class XDocumentAndSource : IXAndSource
    {
        public XDocumentAndSource(string sourceXml, XDocument document, XmlAssertOptions options)
        {
            Ensure.NotNull(document, nameof(document));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            this.SourceXml = sourceXml;
            this.Document = document;
            this.Options = options;
            this.Element = new XElementAndSource(sourceXml, document.Root, options);
        }

        public string SourceXml { get; }

        public XDocument Document { get; }

        public XmlAssertOptions Options { get;  }

        public int LineNumber { get; } = 1;

        public XElementAndSource Element { get;  }
    }
}