namespace Gu.SerializationAsserts.Tests
{
    using Gu.SerializationAsserts.Tests.Dtos;
    using NUnit.Framework;

    public class BinaryFormatterAssertTests
    {
        [Test]
        public void RoundtripHappyPath()
        {
            var dummy = new SerializableDummy();
            var roundtrip = BinaryFormatterAssert.Roundtrip(dummy);
            FieldAssert.Equal(dummy, roundtrip);
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