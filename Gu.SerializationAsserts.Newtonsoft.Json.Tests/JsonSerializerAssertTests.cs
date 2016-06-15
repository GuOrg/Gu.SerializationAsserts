// ReSharper disable RedundantArgumentDefaultValue
namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using System;
    using Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos;
    using NUnit.Framework;

    public class JsonSerializerAssertTests
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

        [Test]
        public void EqualItems()
        {
            var expected = new Dummy { Value = 2 };
            var actual = new Dummy { Value = 2 };
            JsonSerializerAssert.Equal(expected, actual);
        }

        [Test]
        public void NotEqualItems()
        {
            var expected = new Dummy { Value = 1 };
            var actual = new Dummy { Value = 2 };
            var ex = Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual));
            var expectedMessage = "  Json differ at line 1 index 9.\r\n" +
                                  "  Expected: 1| {\"Value\":1}\r\n" +
                                  "  But was:  1| {\"Value\":2}\r\n" +
                                  "  ----------------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void RoundtripDummyWithExpectedJson()
        {
            var actual = new Dummy { Value = 2 };
            var expectedJson = "{\"Value\":2}";
            var roundtrips = new[]
            {
                JsonSerializerAssert.Equal(expectedJson, actual),
                JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Default),
                JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim),
                JsonSerializerAssert.Roundtrip(actual)
            };
            foreach (var roundtrip in roundtrips)
            {
                Assert.AreEqual(2, roundtrip.Value);
            }
        }


        [Test]
        public void RoundtripWithDummyWithExpectedJson()
        {
            var actual = new WithDummy { Dummy = new Dummy { Value = 2 } };
            var expectedJson = "{ Dummy: {\"Value\":2}}";
            var roundtrips = new[]
            {
                JsonSerializerAssert.Equal(expectedJson, actual),
                JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Default),
                JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim),
                JsonSerializerAssert.Roundtrip(actual)
            };
            foreach (var roundtrip in roundtrips)
            {
                Assert.AreEqual(2, roundtrip.Dummy.Value);
            }
        }

        [Test]
        public void RoundtripWithArrayOfInts()
        {
            var ints = new[] { 1, 2, 3 };
            var actual = new WithIntArray { Values = ints };
            var expectedJson = "{\"Values\":[1,2,3]}";
            var roundtrips = new[]
            {
                JsonSerializerAssert.Equal(expectedJson, actual),
                JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Default),
                JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim),
                JsonSerializerAssert.Roundtrip(actual)
            };
            foreach (var roundtrip in roundtrips)
            {
                CollectionAssert.AreEqual(ints, roundtrip.Values);
            }
        }

        [Test]
        public void WithWrongCtorParameter()
        {
            var actual = new WithWrongCtorParameter("meh");
            var expectedJson = "{\"Name\":\"meh\"}";
            var ex = Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expectedJson, actual));
            var expectedMessage = "  Simple roundtrip failed. Source is not equal to roundtripped.\r\n" +
                                  "  AssertException:   Found this difference between expected and actual:\r\n" +
                                  "  expected.<Name>k__BackingField: meh\r\n" +
                                  "    actual.<Name>k__BackingField: null";
            Assert.AreEqual(expectedMessage, ex.Message);
        }
    }
}
