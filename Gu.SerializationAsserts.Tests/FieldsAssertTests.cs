namespace Gu.SerializationAsserts.Tests.Comparers
{
    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public class FieldsAssertTests
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
            var em1 = "  Fields differ first diff:\r\n" +
                      "  expected.value: 1\r\n" +
                      "    actual.value: 2";
            Assert.AreEqual(em1, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(d2, d1));
            var em2 = "  Fields differ first diff:\r\n" +
                      "  expected.value: 2\r\n" +
                      "    actual.value: 1";
            Assert.AreEqual(em2, ex2.Message);
        }

        [Test]
        public void EqualNestedNoLevels()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            FieldAssert.Equal(l1, l1);
            FieldAssert.Equal(l2, l1);
            FieldAssert.Equal(l1, l2);
        }

        [Test]
        public void NotEqualNestedNoLevels()
        {
            var l1 = new Level { Value = 1 };
            var l2 = new Level { Value = 2 };

            var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
            var em1 = "  Fields differ first diff:\r\n" +
                      "  expected.value: 1\r\n" +
                      "    actual.value: 2";
            Assert.AreEqual(em1, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
            var em2 = "  Fields differ first diff:\r\n" +
                      "  expected.value: 2\r\n" +
                      "    actual.value: 1";
            Assert.AreEqual(em2, ex2.Message);

        }

        [Test]
        public void EqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 2 };
            var l2 = new Level { Value = 2 };

            FieldAssert.Equal(l1, l1);
            FieldAssert.Equal(l2, l1);
            FieldAssert.Equal(l1, l2);
        }

        [Test]
        public void EqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 2 } };

            FieldAssert.Equal(l1, l1);
            FieldAssert.Equal(l2, l1);
            FieldAssert.Equal(l1, l2);
        }

        [Test]
        public void NotEqualNestedOneLevel1()
        {
            var l1 = new Level { Value = 1, Next = new Level() };
            var l2 = new Level { Value = 2, Next = new Level() };

            var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
            var em1 = "  Fields differ first diff:\r\n" +
                      "  expected.value: 1\r\n" +
                      "    actual.value: 2";
            Assert.AreEqual(em1, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
            var em2 = "  Fields differ first diff:\r\n" +
                      "  expected.value: 2\r\n" +
                      "    actual.value: 1";
            Assert.AreEqual(em2, ex2.Message);
        }

        [Test]
        public void NotEqualNestedOneLevel2()
        {
            var l1 = new Level { Value = 2, Next = null };
            var l2 = new Level { Value = 2, Next = new Level() };

            var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
            var em1 = "  Fields differ first diff:\r\n" +
                      "  expected.next: null\r\n" +
                      "    actual.next: Gu.SerializationAsserts.Tests.Dtos.Level";
            Assert.AreEqual(em1, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
            var em2 = "  Fields differ first diff:\r\n" +
                      "  expected.next: Gu.SerializationAsserts.Tests.Dtos.Level\r\n" +
                      "    actual.next: null";
            Assert.AreEqual(em2, ex2.Message);
        }

        [Test]
        public void NotEqualNestedOneLevel3()
        {
            var l1 = new Level { Value = 1, Next = new Level { Value = 2 } };
            var l2 = new Level { Value = 1, Next = new Level { Value = 3 } };

            var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
            var em1 = "  Fields differ first diff:\r\n" +
                      "  expected.next.value: 2\r\n" +
                      "    actual.next.value: 3";
            Assert.AreEqual(em1, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
            var em2 = "  Fields differ first diff:\r\n" +
                      "  expected.next.value: 3\r\n" +
                      "    actual.next.value: 2";
            Assert.AreEqual(em2, ex2.Message);
        }

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
        public void NotEqualIEnumerablesOfDummies()
        {
            var l1 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 2 } };
            var l2 = new[] { new Dummy { Value = 1 }, new Dummy { Value = 5 } };

            var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
            var em1 = "  Fields differ first diff:\r\n" +
                      "  expected[1].value: 2\r\n" +
                      "    actual[1].value: 5";
            Assert.AreEqual(em1, ex1.Message);
            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
            var em2 = "  Fields differ first diff:\r\n" +
                      "  expected[1].value: 5\r\n" +
                      "    actual[1].value: 2";
            Assert.AreEqual(em2, ex2.Message);

        }

        [Test]
        public void NotEqualIEnumerablesOfInts()
        {
            var l1 = new[] { 1, 2 };
            var l2 = new[] { 1, 5 };

            Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
            Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
        }

        [Test]
        public void NotEqualIEnumerablesOfIntsLength()
        {
            var l1 = new[] { 1, 2 };
            var l2 = new[] { 1, 2, 3 };

            Assert.Throws<AssertException>(() => FieldAssert.Equal(l1, l2));
            Assert.Throws<AssertException>(() => FieldAssert.Equal(l2, l1));
        }

        [Test]
        public void EqualParentChildren()
        {
            var p1 = new Parent { new Child(1), new Child(2) };
            var p2 = new Parent { new Child(1), new Child(2) };

            FieldAssert.Equal(p1, p1);
            FieldAssert.Equal(p2, p1);
            FieldAssert.Equal(p1, p2);
        }

        [Test]
        public void NotEqualParentChildren()
        {
            var p1 = new Parent { new Child(1), new Child(2) };
            var p2 = new Parent { new Child(1), new Child(5) };
            var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(p2, p1));
            var expectedMessage = "  Fields differ first diff:\r\n" +
                                  "  expected.children[1].<Value>k__BackingField: 5\r\n" +
                                  "    actual.children[1].<Value>k__BackingField: 2";
            Assert.AreEqual(expectedMessage, ex1.Message);
            var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(p1, p2));
        }
    }
}