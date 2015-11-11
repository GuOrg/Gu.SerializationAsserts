namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class XmlAttributeClass : IXmlSerializable
    {
        public int Value { get; set; }

        XmlSchema IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            this.Value = XmlConvert.ToInt32(reader.GetAttribute(nameof(this.Value)));
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(nameof(this.Value), XmlConvert.ToString(this.Value));
        }
    }
}
