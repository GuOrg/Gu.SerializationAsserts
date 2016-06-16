namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using global::Newtonsoft.Json;
    using global::Newtonsoft.Json.Converters;

    using Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos;

    using NUnit.Framework;

    public partial class JsonSerializerAssertTests
    {
        public class Equal
        {
            private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                Converters = new[] { new StringEnumConverter(), }
            };

            [Test]
            public void EqualDummies()
            {
                var expected = new Dummy { Value = 2 };
                var actual = new Dummy { Value = 2 };
                JsonSerializerAssert.Equal(expected, actual);
                JsonSerializerAssert.Equal(expected, actual, this.serializerSettings);
            }

            [Test]
            public void EqualWithDummies()
            {
                var expected = new WithDummy { Dummy = new Dummy { Value = 2 } };
                var actual = new WithDummy { Dummy = new Dummy { Value = 2 } };
                JsonSerializerAssert.Equal(expected, actual);
                JsonSerializerAssert.Equal(expected, actual, this.serializerSettings);
            }

            [Test]
            public void EqualIntArrays()
            {
                var expected = new[] { 1, 2, 3 };
                var actual = new[] { 1, 2, 3 };
                JsonSerializerAssert.Equal(expected, actual);
                JsonSerializerAssert.Equal(expected, actual, this.serializerSettings);
            }

            [Test]
            public void NotEqualDummies()
            {
                var expected = new Dummy { Value = 1 };
                var actual = new Dummy { Value = 2 };

                var exceptions = new[]
                                    {
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual)),
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual, this.serializerSettings)),
                                    };
                var expectedMessage = "  Json differ at line 1 index 9.\r\n" +
                                      "  Expected: 1| {\"Value\":1}\r\n" +
                                      "  But was:  1| {\"Value\":2}\r\n" +
                                      "  ----------------------^";
                foreach (var exception in exceptions)
                {
                    Assert.AreEqual(expectedMessage, exception.Message);
                }
            }

            [Test]
            public void NotEqualWithDummies()
            {
                var expected = new WithDummy { Dummy = new Dummy { Value = 1 } };
                var actual = new WithDummy { Dummy = new Dummy { Value = 2 } };

                var exceptions = new[]
                                    {
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual)),
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual, this.serializerSettings)),
                                    };
                var expectedMessage = "  Json differ at line 1 index 18.\r\n" +
                                      "  Expected: 1| {\"Dummy\":{\"Value\":1}}\r\n" +
                                      "  But was:  1| {\"Dummy\":{\"Value\":2}}\r\n" +
                                      "  -------------------------------^";
                foreach (var exception in exceptions)
                {
                    Assert.AreEqual(expectedMessage, exception.Message);
                }
            }

            [Test]
            public void NotEqualIntArraysDifferentValues()
            {
                var expected = new[] { 1, 2, 3 };
                var actual = new[] { 1, 3, 4 };

                var exceptions = new[]
                                    {
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual)),
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual, this.serializerSettings)),
                                    };
                var expectedMessage = "  Json differ at line 1 index 3.\r\n" +
                                      "  Expected: 1| [1,2,3]\r\n" +
                                      "  But was:  1| [1,3,4]\r\n" +
                                      "  ----------------^";
                foreach (var exception in exceptions)
                {
                    Assert.AreEqual(expectedMessage, exception.Message);
                }
            }

            [Test]
            public void NotEqualIntArraysDifferentLengths()
            {
                var expected = new[] { 1, 2, 3 };
                var actual = new[] { 1, 2 };

                var exceptions = new[]
                                    {
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual)),
                                       Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expected, actual, this.serializerSettings)),
                                    };
                var expectedMessage = "  Json differ at line 1 index 9.\r\n" +
                                      "  Expected: 1| [1,2,3]\r\n" +
                                      "  But was:  1| [1,2]\r\n" +
                                      "  -----------------^";
                foreach (var exception in exceptions)
                {
                    Assert.AreEqual(expectedMessage, exception.Message);
                }
            }

            [Test]
            public void WithWrongCtorParameter()
            {
                var actual = new WithWrongCtorParameter("meh");
                var expectedJson = "{\"Name\":\"meh\"}";
                var exceptions = new[]
                                     {
                                         Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expectedJson, actual)),
                                         Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expectedJson, actual, this.serializerSettings))
                                     };
                var expectedMessage = "  Simple roundtrip failed. Source is not equal to roundtripped.\r\n" +
                                      "  AssertException:   Found this difference between expected and actual:\r\n" +
                                      "  expected.<Name>k__BackingField: meh\r\n" +
                                      "    actual.<Name>k__BackingField: null";
                foreach (var exception in exceptions)
                {
                    Assert.AreEqual(expectedMessage, exception.Message);
                }
            }
        }
    }
}