namespace Gu.SerializationAsserts.Tests.Comparers
{
    using System.Collections;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public class FieldsEqualsComparerTests
    {
        [Test]
        public void EqualDummies()
        {
            var d1 = new Dummy { Value = 1 };
            var d2 = new Dummy { Value = 1 };

            Assert.AreEqual(true, FieldsEqualsComparer<Dummy>.Default.Equals(d1, d1));
            Assert.AreEqual(true, FieldsEqualsComparer<Dummy>.Default.Equals(d1, d2));
            Assert.AreEqual(true, FieldsEqualsComparer<Dummy>.Default.Equals(d2, d1));

            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Dummy>.Default).Compare(d1, d1));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Dummy>.Default).Compare(d1, d2));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Dummy>.Default).Compare(d2, d1));
        }

        [Test]
        public void NotEqualDummies()
        {
            var d1 = new Dummy { Value = 1 };
            var d2 = new Dummy { Value = 2 };

            Assert.AreEqual(false, FieldsEqualsComparer<Dummy>.Default.Equals(d1, d2));
            Assert.AreEqual(false, FieldsEqualsComparer<Dummy>.Default.Equals(d2, d1));

            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Dummy>.Default).Compare(d1, d2));
            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Dummy>.Default).Compare(d2, d1));
        }

        [Test]
        public void EqualNestedNoLevels()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l1, l1));
            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedNoLevels()
        {
            var l1 = new Level { Value = 1 };
            var l2 = new Level { Value = 2 };

            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }

        [Test]
        public void EqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l1, l1));
            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }

        [Test]
        public void EqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 2 } };

            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l1, l1));
            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(true, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l1));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(0, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 1, Next = new Level() };
            var l2 = new Level { Value = 2, Next = new Level() };

            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 2, Next = null };
            var l2 = new Level { Value = 2, Next = new Level() };

            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }

        [Test]
        public void NotEqualNestedOneLevel3()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 3 } };

            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l1, l2));
            Assert.AreEqual(false, FieldsEqualsComparer<Level>.Default.Equals(l2, l1));

            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l1, l2));
            Assert.AreEqual(1, ((IComparer)FieldsEqualsComparer<Level>.Default).Compare(l2, l1));
        }
    }
}
