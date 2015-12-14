namespace Gu.SerializationAsserts.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using Gu.SerializationAsserts.Tests.Dtos;
    using NUnit.Framework;

    public class FieldsComparerTests
    {
        [Test]
        public void EqualDummies()
        {
            var d1 = new Dummy { Value = 1 };
            var d2 = new Dummy { Value = 1 };

            var comparer = FieldComparer<Dummy>.Default;
            Assert.IsTrue(comparer.Equals(d1, d1));
            Assert.IsTrue(comparer.Equals(d1, d2));
            Assert.IsTrue(comparer.Equals(d2, d1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(d1, d1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(d1, d2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(d2, d1));
        }

        [Test]
        public void NotEqualDummies()
        {
            var d1 = new Dummy { Value = 1 };
            var d2 = new Dummy { Value = 2 };

            var comparer = FieldComparer<Dummy>.Default;
            Assert.IsFalse(comparer.Equals(d1, d2));
            Assert.IsFalse(comparer.Equals(d2, d1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(d1, d2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(d2, d1));
        }

        [Test]
        public void EqualNestedNoLevels()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsTrue(comparer.Equals(l1, l1));
            Assert.IsTrue(comparer.Equals(l1, l2));
            Assert.IsTrue(comparer.Equals(l2, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedNoLevels()
        {
            var l1 = new Level { Value = 1 };
            var l2 = new Level { Value = 2 };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void EqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsTrue(comparer.Equals(l1, l1));
            Assert.IsTrue(comparer.Equals(l1, l2));
            Assert.IsTrue(comparer.Equals(l2, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void EqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 2 } };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsTrue(comparer.Equals(l1, l1));
            Assert.IsTrue(comparer.Equals(l1, l2));
            Assert.IsTrue(comparer.Equals(l2, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 1, Next = new Level() };
            var l2 = new Level { Value = 2, Next = new Level() };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 2, Next = null };
            var l2 = new Level { Value = 2, Next = new Level() };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel3()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 3 } };

            var comparer = FieldComparer<Level>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void EqualIEnumerablesOfDummies()
        {
            var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
            var l2 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };

            var comparer = FieldComparer<IEnumerable<Dummy>>.Default;
            Assert.IsTrue(comparer.Equals(l1, l1));
            Assert.IsTrue(comparer.Equals(l1, l2));
            Assert.IsTrue(comparer.Equals(l2, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void EqualIEnumerablesOfInts()
        {
            var l1 = new[] { 1, 2 };
            var l2 = new[] { 1, 2 };

            var comparer = FieldComparer<IEnumerable<int>>.Default;
            Assert.IsTrue(comparer.Equals(l1, l1));
            Assert.IsTrue(comparer.Equals(l1, l2));
            Assert.IsTrue(comparer.Equals(l2, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualIEnumerablesOfDummies()
        {
            var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
            var l2 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 5 } };

            var comparer = FieldComparer<IEnumerable<Dummy>>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualIEnumerablesOfDummiesTwoDiffs()
        {
            var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
            var l2 = new[] { new Dummy { Value = 2 }, new Dummy { Value = 5 } };

            var comparer = FieldComparer<IEnumerable<Dummy>>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualIEnumerablesOfInts()
        {
            var l1 = new[] { 1, 2 };
            var l2 = new[] { 1, 5 };

            var comparer = FieldComparer<IEnumerable<int>>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void NotEqualIEnumerablesOfIntsLength()
        {
            var l1 = new[] { 1, 2 };
            var l2 = new[] { 1, 2, 3 };

            var comparer = FieldComparer<IEnumerable<int>>.Default;
            Assert.IsFalse(comparer.Equals(l1, l2));
            Assert.IsFalse(comparer.Equals(l2, l1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l1, l2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(l2, l1));
        }

        [Test]
        public void EqualParentChildren()
        {
            var p1 = new Parent { new Child(1), new Child(2) };
            var p2 = new Parent { new Child(1), new Child(2) };

            var comparer = FieldComparer<Parent>.Default;
            Assert.IsTrue(comparer.Equals(p1, p1));
            Assert.IsTrue(comparer.Equals(p1, p2));
            Assert.IsTrue(comparer.Equals(p2, p1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(p1, p1));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(p1, p2));
            Assert.AreEqual(0, ((IComparer)comparer).Compare(p2, p1));
        }

        [Test]
        public void NotEqualParentChildren()
        {
            var p1 = new Parent { new Child(1), new Child(2) };
            var p2 = new Parent { new Child(1), new Child(5) };
            var comparer = FieldComparer<Parent>.Default;
            Assert.IsFalse(comparer.Equals(p1, p2));
            Assert.IsFalse(comparer.Equals(p2, p1));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(p1, p2));
            Assert.AreEqual(-1, ((IComparer)comparer).Compare(p2, p1));
        }
    }
}