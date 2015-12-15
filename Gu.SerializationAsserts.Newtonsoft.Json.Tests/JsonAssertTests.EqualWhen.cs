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
                JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.IgnoreElementOrder);
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
                JsonAssert.Equal(json, json, JsonAssertOptions.IgnoreElementOrder);
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
                JsonAssert.Equal(json, json, JsonAssertOptions.IgnoreElementOrder);
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
                JsonAssert.Equal(json, json, JsonAssertOptions.IgnoreElementOrder);
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
                JsonAssert.Equal(json, json, JsonAssertOptions.IgnoreElementOrder);
            }
        }
    }
}