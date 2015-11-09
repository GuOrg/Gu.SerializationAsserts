namespace Gu.SerializationAsserts.Tests.Internals
{
    using System;

    using NUnit.Framework;

    public class TypeExtTests
    {
        [TestCase(typeof(string), true)]
        [TestCase(typeof(TypeExtTests), false)]
        public void IsEquatableTest(Type type, bool expected)
        {
            Assert.AreEqual(expected, type.Implements(typeof(IEquatable<>), type));
        }

        [TestCase(typeof(string), typeof(IEquatable<>), typeof(string), true)]
        [TestCase(typeof(int), typeof(IEquatable<>), typeof(string), false)]
        public void Implements(Type type, Type genericInterface,Type genericParameter, bool expected)
        {
            Assert.AreEqual(expected, type.Implements(genericInterface, genericParameter));
        }
    }
}
