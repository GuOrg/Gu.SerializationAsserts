namespace Gu.SerializationAsserts.Tests
{
    using System;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public partial class BinaryAssertTests
    {
        [Test]
        public void EqualDummies()
        {
            var d1 = new SerializableDummy { Value = 1 };
            var d2 = new SerializableDummy { Value = 1 };

            BinaryAssert.Equal(d1, d1);
            BinaryAssert.Equal(d2, d1);
            BinaryAssert.Equal(d1, d2);
        }

        [Test]
        public void NotEqualDummies()
        {
            var d1 = new SerializableDummy { Value = 1 };
            var d2 = new SerializableDummy { Value = 2 };

            var ex1 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(d1, d2));
            var ex2 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(d2, d1));
            var expected = "  Expected bytes to be equal.\r\n" +
                           "  Bytes differ at index 198.";
            foreach (var ex in new[] { ex1, ex2 })
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [Test]
        public void EqualNestedNoLevels()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            BinaryAssert.Equal(l1, l1);
            BinaryAssert.Equal(l2, l1);
            BinaryAssert.Equal(l1, l2);
        }

        [Test]
        public void NotEqualNestedNoLevels()
        {
            var l1 = new Level { Value = 1 };
            var l2 = new Level { Value = 2 };

            var ex1 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l1, l2));
            var ex2 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l2, l1));
            var expected = "  Expected bytes to be equal.\r\n" +
                           "  Bytes differ at index 410.";
            foreach (var ex in new[] { ex1, ex2 })
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [Test]
        public void EqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            BinaryAssert.Equal(l1, l1);
            BinaryAssert.Equal(l2, l1);
            BinaryAssert.Equal(l1, l2);
        }

        [Test]
        public void EqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 2 } };

            BinaryAssert.Equal(l1, l1);
            BinaryAssert.Equal(l2, l1);
            BinaryAssert.Equal(l1, l2);
        }

        [Test]
        public void NotEqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 1, Next = new Level() };
            var l2 = new Level { Value = 2, Next = new Level() };

            var ex1 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l1, l2));
            var ex2 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l2, l1));
            var expected = "  Expected bytes to be equal.\r\n" +
                           "  Bytes differ at index 410.";
            foreach (var ex in new[] { ex1, ex2 })
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }

        [Test]
        public void NotEqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 2, Next = null };
            var l2 = new Level { Value = 2, Next = new Level() };

            var ex = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l1, l2));
            var expected = "  Expected bytes to have equal lengths.\r\n" +
                           "  expected: 742.\r\n" +
                           "  actual:   787.";
            Assert.AreEqual(expected, ex.Message);
            expected = "  Expected bytes to have equal lengths.\r\n" +
                       "  expected: 787.\r\n" +
                       "  actual:   742.";

            ex = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l2, l1));
            Assert.AreEqual(expected, ex.Message);
        }

        [Test]
        public void NotEqualNestedOneLevel3()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 3 } };

            var ex1 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l1, l2));
            var ex2 = Assert.Throws<AssertException>(() => BinaryAssert.Equal(l2, l1));
            var expected = "  Expected bytes to be equal.\r\n" +
                           "  Bytes differ at index 433.";
            foreach (var ex in new[] { ex1, ex2 })
            {
                Assert.AreEqual(expected, ex.Message);
            }
        }
    }
}
