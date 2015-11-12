namespace Gu.SerializationAsserts.Tests
{
    using System.IO;
    using System.Xml.Serialization;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public class XmlSerializerAssertRoundtripTests
    {
        [Test]
        public void HappyPath()
        {
            var actual = new Dummy { Value = 2 };
            var roundtrip = XmlSerializerAssert.Roundtrip(actual);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void ReadingOutsideEndElementDoesNotThrowWithSimpleRoundtrip()
        {
            // this test is just to show that a simple roundtrip does not catch this.
            var actual = new ReadingOutsideEndElement { Value = 2 };
            var serializer = new XmlSerializer(typeof(ReadingOutsideEndElement));
            string xml = null;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, actual);
                xml = writer.ToString();
            }

            using (var reader = new StringReader(xml))
            {
                var roundtrip = (ReadingOutsideEndElement)serializer.Deserialize(reader);
                Assert.AreEqual(actual.Value, roundtrip.Value);
            }
        }

        [Test]
        public void ReadingOutsideEndElementThrows()
        {
            var actual = new ReadingOutsideEndElement { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => XmlSerializerAssert.Roundtrip(actual));
            var expectedMessage = "  Expected string length 37 but was 177.\r\n"
                      + "  Strings differ at line 1 index 1.\r\n" + "  Expected: 1| <Dummy>\r\n"
                      + "  But was:  1| <?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n"
                      + "  --------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }


        [Test]
        public void RoundtripForgotReadEndElementThrows()
        {
            var actual = new ForgotReadEndElement { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => XmlSerializerAssert.Roundtrip(actual));
            var expectedMessage = "  Expected string length 37 but was 177.\r\n" +
                                  "  Strings differ at line 1 index 1.\r\n" + "  Expected: 1| <Dummy>\r\n" +
                                  "  But was:  1| <?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                                  "  --------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}
