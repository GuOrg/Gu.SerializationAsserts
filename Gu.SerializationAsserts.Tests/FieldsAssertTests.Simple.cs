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
                var x = new Dummy { Value = 1 };
                var y = new Dummy { Value = 1 };

                FieldAssert.Equal(x, x);
                FieldAssert.Equal(y, x);
                FieldAssert.Equal(x, y);
            }

            [Test]
            public void EqualTwoValueDummies()
            {
                var x = new TwoValueDummy("a", 1);
                var y = new TwoValueDummy("a", 1);

                FieldAssert.Equal(x, x);
                FieldAssert.Equal(y, x);
                FieldAssert.Equal(x, y);
            }

            [Test]
            public void NotEqualDummies()
            {
                var x = new Dummy { Value = 1 };
                var y = new Dummy { Value = 2 };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(x, y));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.value: 1\r\n" +
                          "    actual.value: 2";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(y, x));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.value: 2\r\n" +
                          "    actual.value: 1";
                Assert.AreEqual(em2, ex2.Message);
            }

            [Test]
            public void NotEqualTwoValueDummiesOneDiff()
            {
                var x = new TwoValueDummy("a", 1);
                var y = new TwoValueDummy("b", 1);

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(x, y));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.<Name>k__BackingField: a\r\n" +
                          "    actual.<Name>k__BackingField: b";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(y, x));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.<Name>k__BackingField: b\r\n" +
                          "    actual.<Name>k__BackingField: a";
                Assert.AreEqual(em2, ex2.Message);
            }

            [Test]
            public void NotEqualTwoValueDummiesTwoDiffs()
            {
                var x = new TwoValueDummy("a", 1);
                var y = new TwoValueDummy("b", 2);

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(x, y));
                var em1 = "  Fields differ between expected and actual, here are the 2 differences:\r\n" +
                          "  expected.<Name>k__BackingField: a\r\n" +
                          "    actual.<Name>k__BackingField: b\r\n" +
                          "\r\n" +
                          "  expected.<Value>k__BackingField: 1\r\n" +
                          "    actual.<Value>k__BackingField: 2";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(y, x));
                var em2 = "  Fields differ between expected and actual, here are the 2 differences:\r\n" +
                          "  expected.<Name>k__BackingField: b\r\n" +
                          "    actual.<Name>k__BackingField: a\r\n" +
                          "\r\n" +
                          "  expected.<Value>k__BackingField: 2\r\n" +
                          "    actual.<Value>k__BackingField: 1";
                Assert.AreEqual(em2, ex2.Message);
            }
        }
    }
}