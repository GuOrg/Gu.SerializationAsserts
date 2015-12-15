namespace Gu.SerializationAsserts.Tests
{
    using System.IO;
    using System.Xml.Serialization;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public class XmlSerializerAssertTestsRoundtrip
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
        public void SimpleRoundtripReadingOutsideEndElementDoesNotThrow()
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
            var expectedMessage = "  Roundtrip of item in ContainerClass Failed.\r\n" +
                                  "  This means there is an error in serialization.\r\n" +
                                  "  If you are implementing IXmlSerializable check that you handle ReadEndElement properly as it is a common source of bugs.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void SimpleRoundtripForgotReadEndElementDoesNotThrow()
        {
            // this test is just to show that a simple roundtrip does not catch this.
            var actual = new ForgotReadEndElement { Value = 2 };
            var serializer = new XmlSerializer(typeof(ForgotReadEndElement));
            string xml = null;
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, actual);
                xml = writer.ToString();
            }

            using (var reader = new StringReader(xml))
            {
                var roundtrip = (ForgotReadEndElement)serializer.Deserialize(reader);
                Assert.AreEqual(actual.Value, roundtrip.Value);
            }
        }

        [Test]
        public void RoundtripForgotReadEndElementThrows()
        {
            var actual = new ForgotReadEndElement { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => XmlSerializerAssert.Roundtrip(actual));
            var expectedMessage = "  Roundtrip of item in ContainerClass Failed.\r\n" +
                                  "  This means there is an error in serialization.\r\n" +
                                  "  If you are implementing IXmlSerializable check that you handle ReadEndElement properly as it is a common source of bugs.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void RoundtripForgotReadElementThrows()
        {
            var actual = new ForgotReadElement { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => XmlSerializerAssert.Roundtrip(actual));
            var expectedMessage = "  Simple roundtrip failed. Source is not equal to roundtripped.\r\n" +
                                  "  AssertException:   Found this difference between expected and actual:\r\n" +
                                  "  expected.value: 2\r\n" +
                                  "    actual.value: 0";
            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}
