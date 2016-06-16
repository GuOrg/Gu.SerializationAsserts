// ReSharper disable RedundantArgumentDefaultValue
namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using System;
    using Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos;
    using NUnit.Framework;

    public partial class JsonSerializerAssertTests
    {
        [Test]
        public void ToJson()
        {
            var dummy = new Dummy { Value = 2 };
            var json = JsonSerializerAssert.ToJson(dummy);
            Console.Write(json);
            var expected = "{\"Value\":2}";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void ToEscapedJson()
        {
            var dummy = new Dummy { Value = 2 };
            var json = JsonSerializerAssert.ToEscapedJson(dummy);
            var expected = "\"{\\\"Value\\\":2}\"";
            Assert.AreEqual(expected, json);
        }

        [Test]
        public void FromJson()
        {
            var json = "{\"Value\":2}";
            var dummy = JsonSerializerAssert.FromJson<Dummy>(json);
            Assert.AreEqual(2, dummy.Value);
        }
    }
}
