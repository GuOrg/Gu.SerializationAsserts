namespace Gu.SerializationAsserts.Tests
{
    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public partial class FieldsAssertTests
    {
        public class Simple
        {
            [Test]
            public void EqualDummies()
            {
                var d1 = new Dummy { Value = 1 };
                var d2 = new Dummy { Value = 1 };

                FieldAssert.Equal(d1, d1);
                FieldAssert.Equal(d2, d1);
                FieldAssert.Equal(d1, d2);
            }

            [Test]
            public void NotEqualDummies()
            {
                var d1 = new Dummy { Value = 1 };
                var d2 = new Dummy { Value = 2 };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(d1, d2));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.value: 1\r\n" +
                          "    actual.value: 2";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(d2, d1));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.value: 2\r\n" +
                          "    actual.value: 1";
                Assert.AreEqual(em2, ex2.Message);
            }

        }
    }
}