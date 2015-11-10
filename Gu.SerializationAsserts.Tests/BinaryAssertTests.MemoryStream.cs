using System.IO;
using NUnit.Framework;

namespace Gu.SerializationAsserts.Tests
{
    public partial class BinaryAssertTests
    {
        [Test]
        public void EqualsHappyPath()
        {
            var expected = new MemoryStream(new byte[] { 1, 2, 3 });
            var actual = new MemoryStream(new byte[] { 1, 2, 3 });

            BinaryAssert.Equal(expected, actual);
        }

        [Test]
        public void EqualsLengthDiffer()
        {
            var expected = new MemoryStream(new byte[] { 1, 2, 3 });
            var actual = new MemoryStream(new byte[] { 1, 2, 3, 4 });

            var ex = Assert.Throws<AssertException>(() => BinaryAssert.Equal(expected, actual));

            var expectedMessage = "  Expected bytes to have equal lengths.\r\n" +
                                  "  expected: 3.\r\n" +
                                  "  actual:   4.";

            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualsBytesDiffer()
        {
            var expected = new MemoryStream(new byte[] { 1, 2, 3 });
            var actual = new MemoryStream(new byte[] { 1, 2, 5 });
            var ex = Assert.Throws<AssertException>(() => BinaryAssert.Equal(expected, actual));
            var expectedMessage = "  Expected bytes to be equal.\r\n" +
                                  "  Bytes differ at index 2.";
            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}