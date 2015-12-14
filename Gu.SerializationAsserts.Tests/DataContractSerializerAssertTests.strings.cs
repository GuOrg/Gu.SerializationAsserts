namespace Gu.SerializationAsserts.Tests
{
    using System;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public partial class DataContractSerializerAssertTests
    {
        [Test]
        public void ToXml()
        {
            var dummy = new DataContractDummy { Value = 2 };
            var xml = DataContractSerializerAssert.ToXml(dummy);
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<DataContractDummy xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Gu.SerializationAsserts.Tests.Dtos\">\r\n" +
                           "  <Value>2</Value>\r\n" +
                           "</DataContractDummy>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void ToEscapedXml()
        {
            var dummy = new Dummy { Value = 2 };
            var xml = DataContractSerializerAssert.ToEscapedXml(dummy);
            Console.Write(xml);
            var expected = "\"<?xml version=\\\"1.0\\\" encoding=\\\"utf-16\\\"?>\\r\\n\" +\r\n" +
                           "\"<Dummy xmlns:i=\\\"http://www.w3.org/2001/XMLSchema-instance\\\" xmlns=\\\"http://schemas.datacontract.org/2004/07/Gu.SerializationAsserts.Tests.Dtos\\\">\\r\\n\" +\r\n" +
                           "\"  <Value>2</Value>\\r\\n\" +\r\n" +
                           "\"</Dummy>\"";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void FromXml()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                      "<DataContractDummy xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Gu.SerializationAsserts.Tests.Dtos\">\r\n" +
                      "  <Value>2</Value>\r\n" +
                      "</DataContractDummy>";
            var dummy = DataContractSerializerAssert.FromXml<DataContractDummy>(xml);
            Assert.AreEqual(2, dummy.Value);
        }

        [Test]
        public void EqualItems()
        {
            var expected = new DataContractDummy { Value = 2 };
            var actual = new DataContractDummy { Value = 2 };
            DataContractSerializerAssert.Equal(expected, actual);
        }

        [Test]
        public void NotEqualItems()
        {
            var expected = new DataContractDummy { Value = 1 };
            var actual = new DataContractDummy { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => DataContractSerializerAssert.Equal(expected, actual));
            var expectedMessage = "  Xml differ at line 3 index 7.\r\n" +
                                  "  Expected: 3| <Value>1</Value>\r\n" +
                                  "  But was:  3| <Value>2</Value>\r\n" +
                                  "  --------------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void Equal()
        {
            var actual = new DataContractDummy { Value = 2 };
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                              "<DataContractDummy xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Gu.SerializationAsserts.Tests.Dtos\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</DataContractDummy>";
            var roundtrip = DataContractSerializerAssert.Equal(expectedXml, actual);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void EqualXmlAttributeClass()
        {
            var actual = new XmlAttributeClass { Value = 2 };
            var expectedXml = "<XmlAttributeClass Value=\"2\" />";
            var roundtrip = DataContractSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreNamespaces | XmlAssertOptions.IgnoreDeclaration);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void ReadingOutsideEndElementEqual()
        {
            var actual = new ReadingOutsideEndElement { Value = 2 };
            var expectedXml = "<ReadingOutsideEndElement><Value>2</Value></ReadingOutsideEndElement>";
            var ex = Assert.Throws<AssertException>(() => DataContractSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreNamespaces | XmlAssertOptions.IgnoreDeclaration));
            //var expectedMessage = "  Roundtrip of actual in ContainerClass Failed.\r\n" +
            //                      "  This means there is an error in serialization.\r\n" +
            //                      "  If you are implementing IXmlSerializable check that you handle ReadEndElement properly as it is a common source of bugs.";
            //Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualNoDeclarationOrNamespaces()
        {
            var actual = new DataContractDummy { Value = 2 };
            var expectedXml = "<DataContractDummy>\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</DataContractDummy>";
            var roundtrip = DataContractSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreDeclaration | XmlAssertOptions.IgnoreNamespaces);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void EqualWithAttributeAndDeclaration()
        {
            var actual = new DataContractDummy { Value = 2 };
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                              "<DataContractDummy xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Gu.SerializationAsserts.Tests.Dtos\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</DataContractDummy>";
            var roundtrip = DataContractSerializerAssert.Equal(expectedXml, actual);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }
    }
}