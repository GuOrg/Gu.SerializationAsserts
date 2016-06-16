namespace Gu.SerializationAsserts.Tests
{
    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public partial class FieldsAssertTests
    {
        public class Collection
        {
            [Test]
            public void EqualIEnumerablesOfDummies()
            {
                var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
                var l2 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };

                FieldAssert.Equal(l1, l1);
                FieldAssert.Equal(l2, l1);
                FieldAssert.Equal(l1, l2);
            }

            [Test]
            public void EqualIEnumerablesOfInts()
            {
                var l1 = new[] { 1, 2 };
                var l2 = new[] { 1, 2 };

                FieldAssert.Equal(l1, l1);
                FieldAssert.Equal(l2, l1);
                FieldAssert.Equal(l1, l2);
            }

            [Test]
            public void NotEqualArrayOfDummies()
            {
                var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
                var l2 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 5 } };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected[1].value: 2\r\n" +
                          "    actual[1].value: 5";
                Assert.AreEqual(em1, ex1.Message);
                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected[1].value: 5\r\n" +
                          "    actual[1].value: 2";
                Assert.AreEqual(em2, ex2.Message);
            }

            [Test]
            public void NotEqualArrayOfDummiesTwoDiffs()
            {
                var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
                var l2 = new[] { new Dummy { Value = 2 }, new Dummy { Value = 5 } };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
                var em1 = "  Fields differ between expected and actual, here are the 2 differences:\r\n" +
                          "  expected[0].value: 1\r\n" +
                          "    actual[0].value: 2\r\n" +
                          "\r\n" +
                          "  expected[1].value: 2\r\n" +
                          "    actual[1].value: 5";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
                var em2 = "  Fields differ between expected and actual, here are the 2 differences:\r\n" +
                          "  expected[0].value: 2\r\n" +
                          "    actual[0].value: 1\r\n" +
                          "\r\n" +
                          "  expected[1].value: 5\r\n" +
                          "    actual[1].value: 2";
                Assert.AreEqual(em2, ex2.Message);
            }

            [Test]
            public void NotEqualArrayOfIntsOneDiff()
            {
                var l1 = new[] { 1, 2 };
                var l2 = new[] { 1, 5 };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected[1]: 2\r\n" +
                          "    actual[1]: 5";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected[1]: 5\r\n" +
                          "    actual[1]: 2";
                Assert.AreEqual(em2, ex2.Message);
            }

            [Test]
            public void NotEqualArrayOfIntsTwoDiffs()
            {
                var l1 = new[] { 1, 2 };
                var l2 = new[] { 3, 4 };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
                var em1 = "  Fields differ between expected and actual, here are the 2 differences:\r\n" +
                          "  expected[0]: 1\r\n" +
                          "    actual[0]: 3\r\n" +
                          "\r\n" +
                          "  expected[1]: 2\r\n" +
                          "    actual[1]: 4";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
                var em2 = "  Fields differ between expected and actual, here are the 2 differences:\r\n" +
                          "  expected[0]: 3\r\n" +
                          "    actual[0]: 1\r\n" +
                          "\r\n" +
                          "  expected[1]: 4\r\n" +
                          "    actual[1]: 2";
                Assert.AreEqual(em2, ex2.Message);
            }

            [Test]
            public void NotEqualArrayOfIntsLength()
            {
                var l1 = new[] { 1, 2 };
                var l2 = new[] { 1, 2, 3 };

                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.Count: 2\r\n" +
                          "    actual.Count: 3";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected.Count: 3\r\n" +
                          "    actual.Count: 2";
                Assert.AreEqual(em2, ex2.Message);
            }
        }
    }
}