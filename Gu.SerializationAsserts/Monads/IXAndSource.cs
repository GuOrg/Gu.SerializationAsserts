namespace Gu.SerializationAsserts
{
    internal interface IXAndSource
    {
        string SourceXml { get; }
        XmlAssertOptions Options { get; }
        int LineNumber { get; }
    }
}