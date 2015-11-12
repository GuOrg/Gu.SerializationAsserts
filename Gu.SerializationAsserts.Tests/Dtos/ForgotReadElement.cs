namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class ForgotReadElement : IXmlSerializable
    {
        private int value;

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        XmlSchema IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();
            reader.Read();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(this.Value), XmlConvert.ToString(this.Value));
        }
    }
}