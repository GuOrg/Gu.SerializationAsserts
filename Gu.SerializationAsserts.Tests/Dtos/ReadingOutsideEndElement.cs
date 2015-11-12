namespace Gu.SerializationAsserts.Tests.Dtos
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    public class ReadingOutsideEndElement : IXmlSerializable
    {
        public int Value { get; set; }

        XmlSchema IXmlSerializable.GetSchema() => null;

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();
            this.Value = XmlConvert.ToInt32(reader.ReadElementString(nameof(this.Value)));
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                throw new InvalidOperationException();
            }
            reader.Read();
            reader.Read(); // reading once outside the lement
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteElementString(nameof(this.Value), XmlConvert.ToString(this.Value));
        }
    }
}