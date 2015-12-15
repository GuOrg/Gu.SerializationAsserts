namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using Gu.SerializationAsserts.Newtonsoft.Json;
    using NUnit.Framework;

    public partial class JsonAssertTests
    {
        public class NotEqualWhen
        {
            [Test]
            public void ArrayAndObject()
            {
                var expectedJson = "{ \"Values\": [1, 2, 3] }";

                var actualJson = "[1, 2, 3 ]";

                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 1 index 0.\r\n" +
                               "  Expected: 1| { \"Values\": [1, 2, 3] }\r\n" +
                               "  But was:  1| [1, 2, 3 ]\r\n" +
                               "  -------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void WrongPropertyName()
            {
                var expectedJson = "{ \"Value\": 1 }";
                var actualJson = "{ \"Wrong\": 1 }";
                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 1 index 3.\r\n" +
                               "  Expected: 1| { \"Value\": 1 }\r\n" +
                               "  But was:  1| { \"Wrong\": 1 }\r\n" +
                               "  ----------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void WrongPropertyValue()
            {
                var expectedJson = "{ \"Value\": 1 }";
                var actualJson = "{ \"Value\": 2 }";
                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 1 index 11.\r\n" +
                               "  Expected: 1| { \"Value\": 1 }\r\n" +
                               "  But was:  1| { \"Value\": 2 }\r\n" +
                               "  ------------------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void NotEqualWhenInvalidJsonUnmatchedElement()
            {
                var json = "{ \"Value\": 1 ";

                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(json, json));
                var expected = "  expected is not valid Json.\r\n" +
                               "  JsonReaderException: Unexpected end of content while loading JObject. Path 'Value', line 1, position 13.";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void WrongNestedElement()
            {
                var expectedJson = "{\r\n" +
                                   "  \"Dummy\": {\r\n" +
                                   "    \"Value\": 1\r\n" +
                                   "  }\r\n" +
                                   "}";

                var actualJson = "{\r\n" +
                                   "  \"Dummy\": {\r\n" +
                                   "    \"Wrong\": 1\r\n" +
                                   "  }\r\n" +
                                   "}";

                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 3 index 1.\r\n" +
                               "  Expected: 3| \"Value\": 1\r\n" +
                               "  But was:  3| \"Wrong\": 1\r\n" +
                               "  --------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void WrongNestedElementValue()
            {
                var expectedJson = "{\r\n" +
                                   "  \"Dummy\": {\r\n" +
                                   "    \"Value\": 1\r\n" +
                                   "  }\r\n" +
                                   "}";

                var actualJson = "{\r\n" +
                                   "  \"Dummy\": {\r\n" +
                                   "    \"Value\": 2\r\n" +
                                   "  }\r\n" +
                                   "}";

                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 3 index 9.\r\n" +
                               "  Expected: 3| \"Value\": 1\r\n" +
                               "  But was:  3| \"Value\": 2\r\n" +
                               "  ----------------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void NullAndMissingElement()
            {
                var expectedJson = "{ \"A\": null, \"B\": 1 }";
                var actualJson = "{ \"B\": 1 }";

                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim));
                var expected = "  Json differ at line 1 index 3.\r\n" +
                               "  Expected: 1| { \"A\": null, \"B\": 1 }\r\n" +
                               "  But was:  1| { \"B\": 1 }\r\n" +
                               "  ----------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void WrongArrayElementOrder()
            {
                var expectedJson = "{ \"Values\": [ 1, 2, 3 ] }";
                var actualJson = "{ \"Values\": [ 3, 2, 1 ] }";

                var exts = new[]
                {
                    Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson)),
                    Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Default)),
                    Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim))
                };
                var expected = "  Json differ at line 1 index 14.\r\n" +
                               "  Expected: 1| { \"Values\": [ 1, 2, 3 ] }\r\n" +
                               "  But was:  1| { \"Values\": [ 3, 2, 1 ] }\r\n" +
                               "  ---------------------------^";
                foreach (var ext in exts)
                {
                    Assert.AreEqual(expected, ext.Message);
                }
            }

            [Test]
            public void VerbatimChecksOrder()
            {
                var expectedJson = "{\r\n" +
                                   "  \"A\": 1, \r\n" +
                                   "  \"B\": 2 \r\n" +
                                   "}";

                var actualJson = "{\r\n" +
                                 "  \"B\": 2, \r\n" +
                                 "  \"A\": 1 \r\n" +
                                 "}";
                var exception = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim));
                var expected = "  The order of elements is incorrect.\r\n" +
                               "  Json differ at line 2 index 1.\r\n" +
                               "  Expected: 2| \"A\": 1,\r\n" +
                               "  But was:  2| \"B\": 2,\r\n" +
                               "  --------------^";
                Assert.AreEqual(expected, exception.Message);
            }
        }
    }
}
