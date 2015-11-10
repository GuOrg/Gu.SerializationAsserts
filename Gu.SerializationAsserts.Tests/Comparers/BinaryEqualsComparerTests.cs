using Gu.SerializationAsserts.Tests.Dtos;
using NUnit.Framework;

namespace Gu.SerializationAsserts.Tests.Comparers
{
    public class BinaryEqualsComparerTests
    {
        [Test]
        public void EqualDummies()
        {
            var d1 = new SerializableDummy { Value = 1 };
            var d2 = new SerializableDummy { Value = 1 };

            Assert.AreEqual(true, BinaryEqualsComparer<SerializableDummy>.Default.Equals(d1, d1));
            Assert.AreEqual(true, BinaryEqualsComparer<SerializableDummy>.Default.Equals(d1, d2));
            Assert.AreEqual(true, BinaryEqualsComparer<SerializableDummy>.Default.Equals(d2, d1));
        }

        [Test]
        public void NotEqualDummies()
        {
            var d1 = new SerializableDummy { Value = 1 };
            var d2 = new SerializableDummy { Value = 2 };

            Assert.AreEqual(false, BinaryEqualsComparer<SerializableDummy>.Default.Equals(d1, d2));
            Assert.AreEqual(false, BinaryEqualsComparer<SerializableDummy>.Default.Equals(d2, d1));
        }

        [Test]
        public void EqualNestedNoLevels()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l1, l1));
            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }

        [Test]
        public void NotEqualNestedNoLevels()
        {
            var l1 = new Level { Value = 1 };
            var l2 = new Level { Value = 2 };

            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }

        [Test]
        public void EqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l1, l1));
            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }

        [Test]
        public void EqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 2 } };

            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l1, l1));
            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(true, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 1, Next = new Level() };
            var l2 = new Level { Value = 2, Next = new Level() };

            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 2, Next = null };
            var l2 = new Level { Value = 2, Next = new Level() };

            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel3()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 3 } };

            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, BinaryEqualsComparer<Level>.Default.Equals(l2, l1));
        }
    }
}