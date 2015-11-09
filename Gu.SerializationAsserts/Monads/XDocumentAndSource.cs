namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal class XDocumentAndSource
    {
        public XDocumentAndSource(string sourceXml, XDocument document)
        {
            Ensure.NotNull(document, nameof(document));
            Ensure.NotNullOrEmpty(sourceXml, nameof(sourceXml));
            SourceXml = sourceXml;
            Document = document;
            Element = new XElementAndSource(sourceXml, document.Root);
        }

        public string SourceXml { get; }

        public XDocument Document { get; }

        public XElementAndSource Element { get;  } 
    }
}