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
                var expected = "  Json differ at line 1 index 3.\r\n" +
                               "  Expected: 1| { \"Value\": 1 }\r\n" +
                               "  But was:  1| { \"Value\": 2 }\r\n" +
                               "  --------------------------^";
                Assert.AreEqual(expected, exception.Message);
            }

            [Test]
            public void NotEqualWhenInvalidJsonUnmatchedElement()
            {
                var json = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                           "<Dummy>\r\n" +
                           "  <Value>2</Wrong>\r\n" +
                           "</Dummy>";

                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(json, json));
                var expected = "  expected is not valid Json.\r\n" +
                               "  JsonException: The 'Value' start tag on line 3 position 4 does not match the end tag of 'Wrong'. Line 3, position 13.";
                Assert.AreEqual(expected, jsonExt.Message);
            }

            [Test]
            public void NotEqualWhenWrongElement()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Value>2</Value>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Wrong>2</Wrong>\r\n" +
                                 "</Dummy>";

                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 3 index 1.\r\n" +
                               "  Expected: 3| <Value>2</Value>\r\n" +
                               "  But was:  3| <Wrong>2</Wrong>\r\n" +
                               "  --------------^";
                Assert.AreEqual(expected, jsonExt.Message);
            }

            [Test]
            public void NotEqualWhenEmptyAndMissingElement()
            {
                var expectedJsons = new[]
                {
                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                    "  <Value></Value>\r\n" +
                    "</Dummy>",

                    //"<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    //"<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                    //"  <Value />\r\n" +
                    //"</Dummy>",
                };

                var actualJsons = new[]
                {
                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                    "</Dummy>",

                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\" />",
                };
                foreach (var expectedJson in expectedJsons)
                {
                    foreach (var actualJson in actualJsons)
                    {
                        var jsonExt =
                            Assert.Throws<AssertException>(
                                () => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim));
                        var expected = "  Json differ at line 3 index 0.\r\n" +
                                       "  Expected: 3| <Value></Value>\r\n" +
                                       "  But was:  ?| Missing\r\n" +
                                       "  -------------^";
                        Assert.AreEqual(expected, jsonExt.Message);
                    }
                }
            }

            [Test]
            public void EqualTreatEmptyAndMissingElementsAsEqual()
            {
                var expectedJsons = new[]
                {
                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                    "  <Value></Value>\r\n" +
                    "</Dummy>",

                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                    "  <Value />\r\n" +
                    "</Dummy>",
                };

                var actualJsons = new[]
                {
                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                    "</Dummy>",

                    "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                    "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\" />",
                };
                Assert.Fail();
                //foreach (var expectedJson in expectedJsons)
                //{
                //    foreach (var actualJson in actualJsons)
                //    {
                //        JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.TreatEmptyAndMissingElemensAsEqual);
                //        JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.TreatEmptyAndMissingAsEqual);
                //    }
                //}
            }

            [Test]
            public void NotEqualWhenWrongNestedElement()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Outer>\r\n" +
                                   "    <Value>2</Value>\r\n" +
                                   "  </Outer>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Outer>\r\n" +
                                 "    <Wrong>2</Wrong>\r\n" +
                                 "  </Outer>\r\n" +
                                 "</Dummy>";


                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 4 index 1.\r\n" +
                               "  Expected: 4| <Value>2</Value>\r\n" +
                               "  But was:  4| <Wrong>2</Wrong>\r\n" +
                               "  --------------^";
                Assert.AreEqual(expected, jsonExt.Message);
            }

            [Test]
            public void NotEqualWhenWrongElementOrder()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Value1>1</Value1>\r\n" +
                                   "  <Value2>2</Value2>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Value2>2</Value2>\r\n" +
                                 "  <Value1>1</Value1>\r\n" +
                                 "</Dummy>";

                var exts = new[]
                {
                    Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson)),
                    Assert.Throws<AssertException>(
                        () => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim))
                };
                var expected = "  The order of elements is incorrect.\r\n" +
                               "  Json differ at line 3 index 6.\r\n" +
                               "  Expected: 3| <Value1>1</Value1>\r\n" +
                               "  But was:  3| <Value2>2</Value2>\r\n" +
                               "  -------------------^";
                foreach (var ext in exts)
                {
                    Assert.AreEqual(expected, ext.Message);
                }
            }

            [Test]
            public void EqualIgnoreElementOrder()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Value1>1</Value1>\r\n" +
                                   "  <Value2>2</Value2>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Value2>2</Value2>\r\n" +
                                 "  <Value1>1</Value1>\r\n" +
                                 "</Dummy>";
                Assert.Fail();
                JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.IgnoreElementOrder);
                //JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.IgnoreOrder);
            }

            [Test]
            public void NotEqualWhenWrongAttributeOrder()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                                 "</Dummy>";

                var ex1 = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  The order of attributes is incorrect.\r\n" +
                               "  Json differ at line 3 index 17.\r\n" +
                               "  Expected: 3| <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                               "  But was:  3| <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                               "  ------------------------------^";

                Assert.AreEqual(expected, ex1.Message);

                var ex2 =
                    Assert.Throws<AssertException>(
                        () => JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.Verbatim));
                Assert.AreEqual(expected, ex2.Message);
            }

            [Test]
            public void EqualIgnoreAttributeOrder()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                                 "</Dummy>";
                Assert.Fail();
                //JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.IgnoreAttributeOrder);
                //JsonAssert.Equal(expectedJson, actualJson, JsonAssertOptions.IgnoreOrder);
            }

            [Test]
            public void NotEqualWhenWrongNestedElementValue()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Outer Attribute=\"meh\">\r\n" +
                                   "    <Value Attribute=\"1\">2</Value>\r\n" +
                                   "  </Outer>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Outer Attribute=\"meh\">\r\n" +
                                 "    <Value Attribute=\"1\">Wrong</Value>\r\n" +
                                 "  </Outer>\r\n" +
                                 "</Dummy>";

                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 4 index 21.\r\n" +
                               "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                               "  But was:  4| <Value Attribute=\"1\">Wrong</Value>\r\n" +
                               "  ----------------------------------^";
                Assert.AreEqual(expected, jsonExt.Message);
            }

            [Test]
            public void NotEqualWhenWrongNestedAttribute()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Outer Attribute=\"meh\">\r\n" +
                                   "    <Value Attribute=\"1\">2</Value>\r\n" +
                                   "  </Outer>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Outer Attribute=\"meh\">\r\n" +
                                 "    <Value Wrong=\"1\">2</Value>\r\n" +
                                 "  </Outer>\r\n" +
                                 "</Dummy>";

                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 4 index 7.\r\n" +
                               "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                               "  But was:  4| <Value Wrong=\"1\">2</Value>\r\n" +
                               "  --------------------^";
                Assert.AreEqual(expected, jsonExt.Message);
            }

            [Test]
            public void NotEqualWhenWrongNestedAttributeValue()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Outer Attribute=\"meh\">\r\n" +
                                   "    <Value Attribute=\"1\">2</Value>\r\n" +
                                   "  </Outer>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Outer Attribute=\"meh\">\r\n" +
                                 "    <Value Attribute=\"Wrong\">2</Value>\r\n" +
                                 "  </Outer>\r\n" +
                                 "</Dummy>";


                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 4 index 18.\r\n" +
                               "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                               "  But was:  4| <Value Attribute=\"Wrong\">2</Value>\r\n" +
                               "  -------------------------------^";
                Assert.AreEqual(expected, jsonExt.Message);
            }

            [Test]
            public void NotEqualWhenWrongElementValue()
            {
                var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                   "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                   "  <Value>1</Value>\r\n" +
                                   "</Dummy>";

                var actualJson = "<?Json version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                 "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
                                 "  <Value>Wrong</Value>\r\n" +
                                 "</Dummy>";

                var jsonExt = Assert.Throws<AssertException>(() => JsonAssert.Equal(expectedJson, actualJson));
                var expected = "  Json differ at line 3 index 7.\r\n" +
                               "  Expected: 3| <Value>1</Value>\r\n" +
                               "  But was:  3| <Value>Wrong</Value>\r\n" +
                               "  --------------------^";
                Assert.AreEqual(expected, jsonExt.Message);
            }
        }
    }
}
