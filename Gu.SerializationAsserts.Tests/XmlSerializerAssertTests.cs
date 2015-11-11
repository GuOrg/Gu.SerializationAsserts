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
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Value>2</Value>\r\n" +
                           "</Dummy>";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void ToEscapedXml()
        {
            var dummy = new Dummy { Value = 2 };
            var xml = XmlSerializerAssert.ToEscapedXml(dummy);
            var expected = "\"<?xml version=\\\"1.0\\\" encoding=\\\"utf-16\\\"?>\\r\\n\" +\r\n" +
                           "\"<Dummy xmlns:xsd=\\\"http://www.w3.org/2001/XMLSchema\\\" xmlns:xsi=\\\"http://www.w3.org/2001/XMLSchema-instance\\\">\\r\\n\" +\r\n" +
                           "\"  <Value>2</Value>\\r\\n\" +\r\n" +
                           "\"</Dummy>\"";
            Assert.AreEqual(expected, xml);
        }

        [Test]
        public void FromXml()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                      "  <Value>2</Value>\r\n" +
                      "</Dummy>";
            var dummy = XmlSerializerAssert.FromXml<Dummy>(xml);
            Assert.AreEqual(2, dummy.Value);
        }

        [Test]
        public void EqualItems()
        {
            var expected = new DataContractDummy { Value = 2 };
            var actual = new DataContractDummy { Value = 2 };
            XmlSerializerAssert.Equal(expected, actual);
        }

        [Test]
        public void NotEqualItems()
        {
            var expected = new DataContractDummy { Value = 1 };
            var actual = new DataContractDummy { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => XmlSerializerAssert.Equal(expected, actual));
            var expectedMessage = "  String lengths are both 201.\r\n" +
                                  "  Strings differ at line 3 index 7.\r\n" +
                                  "  Expected: 3| <Value>1</Value>\r\n" +
                                  "  But was:  3| <Value>2</Value>\r\n" +
                                  "  --------------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void Equal()
        {
            var actual = new Dummy { Value = 2 };
            var expectedXml = "<Dummy>\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";
            var roundtrip = XmlSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreNameSpaces | XmlAssertOptions.IgnoreDeclaration);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void EqualXmlAttributeClass()
        {
            var actual = new XmlAttributeClass { Value = 2 };
            var expectedXml = "<XmlAttributeClass Value=\"2\" />";
            var roundtrip = XmlSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreNameSpaces | XmlAssertOptions.IgnoreDeclaration);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void ForgotReadEndElement()
        {
            var actual = new ForgotReadEndElement { Value = 2 };
            var expectedXml = "<ForgotReadEndElement><Value>2</Value></ForgotReadEndElement>";
            var ex = Assert.Throws<AssertException>(()=> XmlSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreNameSpaces | XmlAssertOptions.IgnoreDeclaration));
            //Assert.AreEqual("Not sure what to write here", ex.Message);
        }

        [Test]
        public void EqualThrowsOnMissingDeclarationWhenVerbatim()
        {
            var actual = new Dummy { Value = 2 };
            var expectedXml = "<Dummy>\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";
            var ex = Assert.Throws<AssertException>(() => XmlSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.Verbatim));
            var expectedMessage = "  Expected string length 37 but was 177.\r\n" +
                                  "  Strings differ at line 1 index 1.\r\n" +
                                  "  Expected: 1| <Dummy>\r\n" +
                                  "  But was:  1| <?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                                  "  --------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualWithAttributeAndDeclaration()
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

        [Test]
        public void RoundTrip()
        {
            var actual = new Dummy { Value = 2 };
            var roundtrip = XmlSerializerAssert.RoundTrip(actual);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }
    }
}
