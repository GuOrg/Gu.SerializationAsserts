namespace Gu.SerializationAsserts.Tests
{

    using NUnit.Framework;

    public class XmlAssertTests
    {
        [Test]
        public void HappyPath()
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""1"">2</Value>
  </Outer>  
</Dummy>";
            XmlAssert.Equal(xml, xml);
        }

        [Test]
        public void WrongEncoding()
        {
            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?><Dummy />";
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-16""?><Dummy />";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  Expected string length 48 but was 47.
  Strings differ at line 1 index 34.
  Expected: <?xml version=""1.0"" encoding=""utf-16""?><Dummy />
  But was:  <?xml version=""1.0"" encoding=""utf-8""?><Dummy />
  --------------------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongRoot()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Wrong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value>2</Value>\r\n" +
                            "</Wrong>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  String lengths are both 176.\r\n" +
                           "  Strings differ at line 2 index 1.\r\n" +
                           "  Expected: <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  But was:  <Wrong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  -----------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void InvalidXmlStartingWithWhiteSpace()
        {
            var xml = " <?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                      "  <Value>2</Value>\r\n" +
                      "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(xml, xml));
            var expected = "  expected is not valid xml.\r\n" +
                           "  XmlException: Unexpected XML declaration. The XML declaration must be the first node in the document, and no white space characters are allowed to appear before it. Line 1, position 4.";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void InvalidXmlUnmatchedElement()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                      "<Dummy>\r\n" +
                      "  <Value>2</Wrong>\r\n" +
                      "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(xml, xml));
            var expected = "  expected is not valid xml.\r\n" +
                           "  XmlException: The 'Value' start tag on line 3 position 4 does not match the end tag of 'Wrong'. Line 3, position 13.";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongElement()
        {
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Value>2</Value>
</Dummy>";

            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Wrong>2</Wrong>
</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  String lengths are both 176.
  Strings differ at line 3 index 1.
  Expected: <Value>2</Value>
  But was:  <Wrong>2</Wrong>
  -----------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongNestedElement()
        {
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""1"">2</Value>
  </Outer>  
</Dummy>";

            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Wrong Attribute=""1"">2</Wrong>
  </Outer>  
</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  String lengths are both 233.
  Strings differ at line 4 index 1.
  Expected: <Value Attribute=""1"">2</Value>
  But was:  <Wrong Attribute=""1"">2</Wrong>
  -----------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongNestedElementValue()
        {
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""1"">2</Value>
  </Outer>  
</Dummy>";

            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""1"">Wrong</Value>
  </Outer>  
</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  Expected string length 233 but was 237.
  Strings differ at line 4 index 21.
  Expected: <Value Attribute=""1"">2</Value>
  But was:  <Value Attribute=""1"">Wrong</Value>
  -------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongNestedAttribute()
        {
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""1"">2</Value>
  </Outer>  
</Dummy>";

            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Wrong=""1"">2</Value>
  </Outer>  
</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  Expected string length 233 but was 229.
  Strings differ at line 4 index 7.
  Expected: <Value Attribute=""1"">2</Value>
  But was:  <Value Wrong=""1"">2</Value>
  -----------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongNestedAttributeValue()
        {
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""1"">2</Value>
  </Outer>  
</Dummy>";

            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Outer Attribute=""meh"">
    <Value Attribute=""Wrong"">2</Value>
  </Outer>  
</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  Expected string length 233 but was 237.
  Strings differ at line 4 index 18.
  Expected: <Value Attribute=""1"">2</Value>
  But was:  <Value Attribute=""Wrong"">2</Value>
  ----------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void WrongElementValue()
        {
            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Value>1</Value>
</Dummy>";

            var actualXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Dummy xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Value>Wrong</Value>
</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = @"  Expected string length 176 but was 180.
  Strings differ at line 3 index 7.
  Expected: <Value>1</Value>
  But was:  <Value>Wrong</Value>
  -----------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }
    }
}
