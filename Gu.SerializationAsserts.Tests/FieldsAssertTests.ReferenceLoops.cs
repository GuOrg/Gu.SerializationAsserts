namespace Gu.SerializationAsserts.Tests
{
    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public partial class FieldsAssertTests
    {
        public class ReferenceLoops
        {
            [Test]
            public void EqualParentChildren()
            {
                var p1 = new Parent { new Child(1), new Child(2) };
                var p2 = new Parent { new Child(1), new Child(2) };
                Assert.AreSame(p1, p1.Children[0].Parent);
                FieldAssert.Equal(p1, p1);
                FieldAssert.Equal(p2, p1);
                FieldAssert.Equal(p1, p2);
            }

            [Test]
            public void NotEqualParentChildren()
            {
                var p1 = new Parent { new Child(1), new Child(2) };
                var p2 = new Parent { new Child(1), new Child(5) };
                var ex1 = Assert.Throws<AssertException>(() => FieldAssert.Equal(p1, p2));
                var em1 = "  Found this difference between expected and actual:\r\n" +
                          "  expected[1].value: 2\r\n" +
                          "    actual[1].value: 5";
                Assert.AreEqual(em1, ex1.Message);

                var ex2 = Assert.Throws<AssertException>(() => FieldAssert.Equal(p2, p1));
                var em2 = "  Found this difference between expected and actual:\r\n" +
                          "  expected[1].value: 5\r\n" +
                          "    actual[1].value: 2";
                Assert.AreEqual(em2, ex2.Message);
            }
        }
    }
}