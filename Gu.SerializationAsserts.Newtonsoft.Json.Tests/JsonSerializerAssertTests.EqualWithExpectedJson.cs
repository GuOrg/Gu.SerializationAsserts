// ReSharper disable RedundantArgumentDefaultValue
namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using global::Newtonsoft.Json;
    using global::Newtonsoft.Json.Converters;

    using Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos;

    using NUnit.Framework;

    public partial class JsonSerializerAssertTests
    {
        public class EqualWithExpectedJson
        {
            private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters = new[] { new StringEnumConverter(), }
            };

            [Test]
            public void Dummy()
            {
                var actual = new Dummy { Value = 2 };
                var expectedJson = "{\"Value\":2}";

                var roundtrips = new[]
                                     {
                                         JsonSerializerAssert.Equal(expectedJson, actual),
                                         JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Default),
                                         JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim),
                                         JsonSerializerAssert.Equal(expectedJson, actual, this.serializerSettings, JsonAssertOptions.Verbatim),
                                     };
                foreach (var roundtrip in roundtrips)
                {
                    Assert.AreNotSame(actual, roundtrip);
                    Assert.AreEqual(2, roundtrip.Value);
                }
            }

            [Test]
            public void WithDummy()
            {
                var actual = new WithDummy { Dummy = new Dummy { Value = 2 } };
                var expectedJson = "{ Dummy: {\"Value\":2}}";

                var roundtrips = new[]
                                     {
                                         JsonSerializerAssert.Equal(expectedJson, actual),
                                         JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Default),
                                         JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim),
                                         JsonSerializerAssert.Equal(expectedJson, actual, this.serializerSettings, JsonAssertOptions.Verbatim),
                                     };
                foreach (var roundtrip in roundtrips)
                {
                    Assert.AreNotSame(actual, roundtrip);
                    Assert.AreEqual(2, roundtrip.Dummy.Value);
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
                                 Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expectedJson, actual, this.serializerSettings, JsonAssertOptions.Verbatim))
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