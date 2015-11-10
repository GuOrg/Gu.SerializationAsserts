namespace Gu.SerializationAsserts
{
    using System.Xml.Linq;

    internal class XDocumentAndSource
    {
        public XDocumentAndSource(string sourceXml, XDocument document)
        {
            Ensure.NotNull(document, nameof(document));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            this.SourceXml = sourceXml;
            this.Document = document;
            this.Element = new XElementAndSource(sourceXml, document.Root);
        }

        public string SourceXml { get; }

        public XDocument Document { get; }

        public XElementAndSource Element { get;  }
    }
}