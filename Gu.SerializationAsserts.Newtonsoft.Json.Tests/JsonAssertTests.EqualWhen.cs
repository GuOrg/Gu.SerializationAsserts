namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using NUnit.Framework;

    public partial class JsonAssertTests
    {
        public class EqualWhen
        {
            [TestCase("{\"Value\":2}", "{\"Value\":2}")]
            [TestCase("{\"Value\":   2   }", "{\"Value\":2}")]
            [TestCase("   {   \"Value\"   :   2   }   ", "{\"Value\":2}")]
            [TestCase("   {\r\n   \"Value\"   :   2 \r\n  }   ", "{\"Value\":2}")]
            public void SimpleObject(string expectedJson, string actualJson)
            {
                JsonAssert.Equal(expectedJson, actualJson);
                JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim);
                JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Default);
            }

            [Test]
            public void WithIntArray()
            {
                const string json = "{" +
                                    "  \"Values\": [" +
                                    "    1," +
                                    "    2," +
                                    "    3" +
                                    "  ]" +
                                    "}";

                JsonAssert.Equal(json, json);
                JsonAssert.Equal(json, json, JsonAssertOptions.Verbatim);
                JsonAssert.Equal(json, json, JsonAssertOptions.Default);
            }

            [Test]
            public void ArrayOfInts()
            {
                const string json = "[" +
                                    "  1," +
                                    "  2," +
                                    "  3" +
                                    "]";

                JsonAssert.Equal(json, json);
                JsonAssert.Equal(json, json, JsonAssertOptions.Verbatim);
                JsonAssert.Equal(json, json, JsonAssertOptions.Default);
            }

            [Test]
            public void WithDummyArray()
            {
                const string json = "{" +
                                    "  \"Values\": [" +
                                    "    {" +
                                    "      \"Value\": 1" +
                                    "    }," +
                                    "    {" +
                                    "      \"Value\": 2" +
                                    "    }," +
                                    "    {" +
                                    "      \"Value\": 3" +
                                    "    }" +
                                    "  ]" +
                                    "}";

                JsonAssert.Equal(json, json);
                JsonAssert.Equal(json, json, JsonAssertOptions.Verbatim);
                JsonAssert.Equal(json, json, JsonAssertOptions.Default);
            }

            [Test]
            public void ArrayOfDummies()
            {
                const string json = "[" +
                                    "  {" +
                                    "    \"Value\": 1" +
                                    "  }," +
                                    "  {" +
                                    "    \"Value\": 2" +
                                    "  }," +
                                    "  {" +
                                    "    \"Value\": 3" +
                                    "  }" +
                                    "]";

                JsonAssert.Equal(json, json);
                JsonAssert.Equal(json, json, JsonAssertOptions.Verbatim);
                JsonAssert.Equal(json, json, JsonAssertOptions.Default);
            }

            [Test]
            public void IgnoresOrder()
            {
                var expectedJson = "{\r\n" +
                                   "  \"A\": 1, \r\n" +
                                   "  \"B\": 2 \r\n" +
                                   "}";

                var actualJson = "{\r\n" +
                                 "  \"B\": 2, \r\n" +
                                 "  \"A\": 1 \r\n" +
                                 "}";
                JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Default);
                JsonAssert.Equal(expectedJson, actualJson);
            }

            [Test]
            public void NullAndMissingElement()
            {
                var expectedJson = "{ \"A\": null, \"B\": 1 }";
                var actualJson = "{ \"B\": 1 }";
                Assert.Inconclusive("dunno if we want this");
                //JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.TreatNullAndMissingAsEqual);
            }
        }
    }
}