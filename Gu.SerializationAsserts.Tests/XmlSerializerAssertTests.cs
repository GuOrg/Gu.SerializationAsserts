namespace Gu.SerializationAsserts.Tests
{
    using System;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public class XmlSerializerAssertTests
    {
        [Test]
        public void ToXml()
        {
            var dummy = new Dummy { Value = 2 };
            var xml = XmlSerializerAssert.ToXml(dummy);
            Console.Write(xml);
            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Value>2</Value>
</Dummy>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void ToEscapedXml()
        {
            var dummy = new Dummy { Value = 2 };
            var xml = XmlSerializerAssert.ToEscapedXml(dummy);
            Console.Write(xml);
            var expected = "\"<?xml version=\\\"1.0\\\" encoding=\\\"utf-16\\\"?>\\r\\n\" +\r\n" +
                           "\"<Dummy xmlns:xsd=\\\"http://www.w3.org/2001/XMLSchema\\\" xmlns:xsi=\\\"http://www.w3.org/2001/XMLSchema-instance\\\">\\r\\n\" +\r\n" +
                           "\"  <Value>2</Value>\\r\\n\" +\r\n" +
                           "\"</Dummy>\"";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void FromXml()
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Value>2</Value>
</Dummy>";
            var dummy = XmlSerializerAssert.FromXml<Dummy>(xml);
            Assert.AreEqual(2, dummy.Value);
        }

        [Test]
        public void Roundtrip()
        {
            var actual = new Dummy { Value = 2 };
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";
            var roundtrip = XmlSerializerAssert.Equal(expectedXml, actual);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }
    }
}
