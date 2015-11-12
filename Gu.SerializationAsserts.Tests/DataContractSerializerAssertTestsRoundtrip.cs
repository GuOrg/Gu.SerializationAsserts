namespace Gu.SerializationAsserts.Tests
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using Gu.SerializationAsserts.Tests.Dtos;
    using NUnit.Framework;

    public class DataContractSerializerAssertTestsRoundtrip
    {
        [Test]
        public void HappyPath()
        {
            var actual = new Dummy { Value = 2 };
            var roundtrip = DataContractSerializerAssert.Roundtrip(actual);
            Assert.AreEqual(roundtrip.Value, actual.Value);
            FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void SimpleRoundtripReadingOutsideEndElementDoesNotThrow()
        {
            // this test is just to show that a simple roundtrip does not catch this.
            var actual = new ReadingOutsideEndElement { Value = 2 };
            var serializer = new DataContractSerializer(typeof(ReadingOutsideEndElement));
            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder))
            {
                serializer.WriteObject(writer, actual);
            }
            var xml = stringBuilder.ToString();

            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                var roundtrip = (ReadingOutsideEndElement)serializer.ReadObject(reader);
                Assert.AreEqual(actual.Value, roundtrip.Value);
            }
        }

        [Test]
        public void ReadingOutsideEndElementThrows()
        {
            var actual = new ReadingOutsideEndElement { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => DataContractSerializerAssert.Roundtrip(actual));
            //var expectedMessage = "  Roundtrip of item in ContainerClass Failed.\r\n" +
            //                      "  This means there is an error in serialization.\r\n" +
            //                      "  If you are implementing IXmlSerializable check that you handle ReadEndElement properly as it is a common source of bugs.";
            //Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void SimpleRoundtripForgotReadEndElementDoesNotThrow()
        {
            // this test is just to show that a simple roundtrip does not catch this.
            var actual = new ForgotReadEndElement { Value = 2 };
            var serializer = new DataContractSerializer(typeof(ForgotReadEndElement));
            var stringBuilder = new StringBuilder();
            using (var writer = XmlWriter.Create(stringBuilder))
            {
                serializer.WriteObject(writer, actual);
            }

            var xml = stringBuilder.ToString();
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                var roundtrip = (ForgotReadEndElement)serializer.ReadObject(reader);
                Assert.AreEqual(actual.Value, roundtrip.Value);
            }
        }
    }
}