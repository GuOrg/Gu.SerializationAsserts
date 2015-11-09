using System.IO;
using Gu.SerializationAsserts.Tests.Dtos;
using NUnit.Framework;

namespace Gu.SerializationAsserts.Tests
{
    public class BinaryFormatterAssertTests
    {
        [Test]
        public void EqualsHappyPath()
        {
            var expected = new MemoryStream(new byte[] { 1, 2, 3 });
            var actual = new MemoryStream(new byte[] { 1, 2, 3 });
            BinaryFormatterAssert.Equals(expected, actual);
        }

        [Test]
        public void EqualsLengthDiffer()
        {
            var expected = new MemoryStream(new byte[] { 1, 2, 3 });
            var actual = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var ex = Assert.Throws<AssertException>(() => BinaryFormatterAssert.Equals(expected, actual));
            var expectedMessage = "  Expected stream lengthts to be equal.\r\n" +
                                  "  expected: 3.\r\n" +
                                  "  actual:   4.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualsBytesDiffer()
        {
            var expected = new MemoryStream(new byte[] { 1, 2, 3 });
            var actual = new MemoryStream(new byte[] { 1, 2, 5 });
            var ex = Assert.Throws<AssertException>(() => BinaryFormatterAssert.Equals(expected, actual));
            var expectedMessage = "  Expected streams to be equal.\r\n" +
                                  "  Streams differ at index 2";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void RoundtripHappyPath()
        {
            var dummy = new SerializableDummy();
            var roundtrip = BinaryFormatterAssert.Roundtrip(dummy);
            FieldAssert.Equals(dummy, roundtrip);
        }

        [Test]
        public void RoundtripMissingSerializable()
        {
            var dummy = new Dummy();
            var ex = Assert.Throws<AssertException>(() => BinaryFormatterAssert.Roundtrip(dummy));
            var expected = "  Writing Dummy to a stream failed.\r\n" +
                           "  SerializationException: Type 'Gu.SerializationAsserts.Tests.Dtos.Dummy' in Assembly 'Gu.SerializationAsserts.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null' is not marked as serializable.";
            Assert.AreEqual(expected, ex.Message);
        }
    }
}