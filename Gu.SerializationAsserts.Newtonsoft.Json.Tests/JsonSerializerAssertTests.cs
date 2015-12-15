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
            var expectedMessage = "  Json differ at line 1 index 7.\r\n" +
                                  "  Expected: 1| {\"Value\":1}\r\n" +
                                  "  But was:  1| {\"Value\":2}\r\n" +
                                  "  ------------------------^";
            Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void Equal()
        {
            Assert.Fail();
            //var actual = new Dummy { Value = 2 };
            //var expectedJson = "<Dummy>\r\n" +
            //                  "  <Value>2</Value>\r\n" +
            //                  "</Dummy>";
            //var roundtrip = JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.IgnoreNamespaces | JsonAssertOptions.IgnoreDeclaration);
            //Assert.AreEqual(roundtrip.Value, actual.Value);
            //FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void EqualJsonAttributeClass()
        {
            Assert.Fail();
            //var actual = new JsonAttributeClass { Value = 2 };
            //var expectedJson = "<JsonAttributeClass Value=\"2\" />";
            //var roundtrip = JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.IgnoreNamespaces | JsonAssertOptions.IgnoreDeclaration);
            //Assert.AreEqual(roundtrip.Value, actual.Value);
            //FieldAssert.Equal(actual, roundtrip);
        }

        [Test]
        public void EqualForgotReadEndElementThrows()
        {
            Assert.Fail();
            //var actual = new ForgotReadEndElement { Value = 2 };
            //var expectedJson = "<ForgotReadEndElement><Value>2</Value></ForgotReadEndElement>";
            //var ex = Assert.Throws<AssertException>(()=> JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.IgnoreNamespaces | JsonAssertOptions.IgnoreDeclaration));
            //var expectedMessage = "  Roundtrip of item in ContainerClass Failed.\r\n" +
            //                      "  This means there is an error in serialization.\r\n" +
            //                      "  If you are implementing IJsonSerializable check that you handle ReadEndElement properly as it is a common source of bugs.";
            //Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualReadingOutsideEndElementThrows()
        {
            Assert.Fail();
            //var actual = new ReadingOutsideEndElement { Value = 2 };
            //var expectedJson = "<ReadingOutsideEndElement><Value>2</Value></ReadingOutsideEndElement>";
            //var ex = Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.IgnoreNamespaces | JsonAssertOptions.IgnoreDeclaration));
            //var expectedMessage = "  Roundtrip of item in ContainerClass Failed.\r\n" +
            //                      "  This means there is an error in serialization.\r\n" +
            //                      "  If you are implementing IJsonSerializable check that you handle ReadEndElement properly as it is a common source of bugs.";
            //Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualThrowsOnMissingDeclarationWhenVerbatim()
        {
            Assert.Fail();
            //var actual = new Dummy { Value = 2 };
            //var expectedJson = "<Dummy>\r\n" +
            //                  "  <Value>2</Value>\r\n" +
            //                  "</Dummy>";
            //var ex = Assert.Throws<AssertException>(() => JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim));
            //var expectedMessage = "  Json differ at line 1 index 1.\r\n" +
            //                      "  Expected: 1| <Dummy>\r\n" +
            //                      "  But was:  1| <?Json version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
            //                      "  --------------^";
            //Assert.AreEqual(expectedMessage, ex.Message);
        }

        [Test]
        public void EqualWithAttributeAndDeclaration()
        {
            Assert.Fail();
            //var actual = new Dummy { Value = 2 };
            //var expectedJson = "<?Json version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
            //                  "<Dummy Jsonns:xsd=\"http://www.w3.org/2001/JsonSchema\" Jsonns:xsi=\"http://www.w3.org/2001/JsonSchema-instance\">\r\n" +
            //                  "  <Value>2</Value>\r\n" +
            //                  "</Dummy>";
            //var roundtrip = JsonSerializerAssert.Equal(expectedJson, actual);
            //Assert.AreEqual(roundtrip.Value, actual.Value);
            //FieldAssert.Equal(actual, roundtrip);
        }
    }
}
