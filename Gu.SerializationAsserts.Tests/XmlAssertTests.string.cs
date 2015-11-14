namespace Gu.SerializationAsserts.Tests
{
    using System;

    using NUnit.Framework;

    public partial class XmlAssertTests
    {
        [Test]
        public void HappyPath()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                      "  <Outer Attribute=\"meh\">" +
                      "    <Value Attribute=\"1\">2</Value>" +
                      "  </Outer>  " +
                      "</Dummy>";
            XmlAssert.Equal(xml, xml);
        }

        [Test]
        public void EqualWhenArrayVerbatim()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                      "<ArrayOfDummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                      "  <Dummy>\r\n" +
                      "    <Value>1</Value>\r\n" +
                      "  </Dummy>\r\n" +
                      "  <Dummy>\r\n" +
                      "    <Value>1</Value>\r\n" +
                      "  </Dummy>\r\n" +
                      "</ArrayOfDummy>";
            XmlAssert.Equal(xml, xml, XmlAssertOptions.Verbatim);
        }

        [Test]
        public void EqualWhenVerbatim()
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                      "  <Outer Attribute=\"meh\">" +
                      "    <Value Attribute=\"1\">2</Value>" +
                      "  </Outer>  " +
                      "</Dummy>";
            XmlAssert.Equal(xml, xml, XmlAssertOptions.Verbatim);
        }

        [Test]
        public void EqualWhenIgnoreDeclaration()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                           "  <Outer Attribute=\"meh\">" +
                           "    <Value Attribute=\"1\">2</Value>" +
                           "  </Outer>  " +
                           "</Dummy>";

            var actual =
                "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                "  <Outer Attribute=\"meh\">" +
                "    <Value Attribute=\"1\">2</Value>" +
                "  </Outer>  " +
                "</Dummy>";


            XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreDeclaration);
        }

        [Test]
        public void NotEqualWhenMissingDeclaration()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Outer Attribute=\"meh\">\r\n" +
                           "    <Value Attribute=\"1\">2</Value>\r\n" +
                           "  </Outer>\r\n" +
                           "</Dummy>";

            var actual =
                "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                "  <Outer Attribute=\"meh\">\r\n" +
                "    <Value Attribute=\"1\">2</Value>\r\n" +
                "  </Outer>\r\n" +
                "</Dummy>";

            var ext = Assert.Throws<AssertException>(() => XmlAssert.Equal(expected, actual));
            var em = "  Expected and actual xml are not equal.\r\n" +
                     "  Xml differ at line 1 index 1.\r\n" +
                     "  Expected: 1| <?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                     "  But was:  1| <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                     "  --------------^";
            Console.WriteLine(ext.Message);
            Assert.AreEqual(em, ext.Message);
        }

        [Test]
        public void EqualWhenIgnoreNamespaces()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Outer Attribute=\"meh\">\r\n" +
                           "    <Value Attribute=\"1\">2</Value>\r\n" +
                           "  </Outer>\r\n" +
                           "</Dummy>";

            var actual = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                         "<Dummy>\r\n" +
                         "  <Outer Attribute=\"meh\">\r\n" +
                         "    <Value Attribute=\"1\">2</Value>\r\n" +
                         "  </Outer>\r\n" +
                         "</Dummy>";

            XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreNamespaces);
        }

        [Test]
        public void NotEqualWhenWrongEncoding()
        {
            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Dummy />";
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><Dummy />";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 1 index 34.\r\n" +
                           "  Expected: 1| <?xml version=\"1.0\" encoding=\"utf-16\"?><Dummy />\r\n" +
                           "  But was:  1| <?xml version=\"1.0\" encoding=\"utf-8\"?><Dummy />\r\n" +
                           "  -----------------------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongRoot()
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
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 2 index 1.\r\n" +
                           "  Expected: 2| <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  But was:  2| <Wrong xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongRootIgnoreDeclaration()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy>\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<Wrong>\r\n" +
                            "  <Value>2</Value>\r\n" +
                            "</Wrong>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreDeclaration));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 2 index 1.\r\n" +
                           "  Expected: 2| <Dummy>\r\n" +
                           "  But was:  1| <Wrong>\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongNamespaces()
        {
            var expected = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                           "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                           "  <Outer Attribute=\"meh\">\r\n" +
                           "    <Value Attribute=\"1\">2</Value>\r\n" +
                           "  </Outer>\r\n" +
                           "</Dummy>";

            var actual = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
                         "<Dummy>\r\n" +
                         "  <Outer Attribute=\"meh\">\r\n" +
                         "    <Value Attribute=\"1\">2</Value>\r\n" +
                         "  </Outer>\r\n" +
                         "</Dummy>";

            var ex = Assert.Throws<AssertException>(() => XmlAssert.Equal(expected, actual, XmlAssertOptions.Verbatim));

            var em = "  Expected and actual xml are not equal.\r\n" +
                     "  Number of attributes does not macth for element: Dummy\r\n" +
                     "  Expected: 2\r\n" +
                     "  But was:  0\r\n" +
                     "  Xml differ at line 2 index 6.\r\n" +
                     "  Expected: 2| <Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                     "  But was:  2| <Dummy>\r\n" +
                     "  -------------------^";
            Assert.AreEqual(em, ex.Message);
        }

        [Test]
        public void NotEqualWhenInvalidXmlStartingWithWhiteSpace()
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
        public void NotEqualWhenInvalidXmlUnmatchedElement()
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
        public void NotEqualWhenWrongElement()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>2</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Wrong>2</Wrong>\r\n" +
                            "</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 3 index 1.\r\n" +
                           "  Expected: 3| <Value>2</Value>\r\n" +
                           "  But was:  3| <Wrong>2</Wrong>\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenEmptyAndMissingElement()
        {
            var expectedXmls = new[]
                                   {
                                      "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <Value></Value>\r\n" +
                                      "</Dummy>",

                                      //"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                      //"<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      //"  <Value />\r\n" +
                                      //"</Dummy>",
                                   };

            var actualXmls = new[]
                                 {
                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                     "</Dummy>",

                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />",
                                 };
            foreach (var expectedXml in expectedXmls)
            {
                foreach (var actualXml in actualXmls)
                {
                    var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim));
                    var expected = "  Expected and actual xml are not equal.\r\n" +
                                   "  Xml differ at line 3 index 0.\r\n" +
                                   "  Expected: 3| <Value></Value>\r\n" +
                                   "  But was:  ?| Missing\r\n" +
                                   "  -------------^";
                    Assert.AreEqual(expected, xmlExt.Message);
                }
            }
        }

        [Test]
        public void EqualTreatEmptyAndMissingElementsAsEqual()
        {
            var expectedXmls = new[]
                                   {
                                      "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <Value></Value>\r\n" +
                                      "</Dummy>",

                                      "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                      "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                      "  <Value />\r\n" +
                                      "</Dummy>",
                                   };

            var actualXmls = new[]
                                 {
                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                                     "</Dummy>",

                                     "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                     "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />",
                                 };
            foreach (var expectedXml in expectedXmls)
            {
                foreach (var actualXml in actualXmls)
                {
                    XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual);
                    XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.TreatEmptyAndMissingAsEqual);
                }
            }
        }

        [Test]
        public void NotEqualWhenWrongNestedElement()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer>\r\n" +
                              "    <Value>2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer>\r\n" +
                            "    <Wrong>2</Wrong>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 4 index 1.\r\n" +
                           "  Expected: 4| <Value>2</Value>\r\n" +
                           "  But was:  4| <Wrong>2</Wrong>\r\n" +
                           "  --------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongElementOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1>1</Value1>\r\n" +
                              "  <Value2>2</Value2>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value2>2</Value2>\r\n" +
                            "  <Value1>1</Value1>\r\n" +
                            "</Dummy>";

            var exts = new[]
                           {
                               Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml)),
                               Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim))
                           };
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  The order of elements is incorrect.\r\n" +
                           "  Xml differ at line 3 index 6.\r\n" +
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
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1>1</Value1>\r\n" +
                              "  <Value2>2</Value2>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value2>2</Value2>\r\n" +
                            "  <Value1>1</Value1>\r\n" +
                            "</Dummy>";
            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreElementOrder);
            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreOrder);
        }

        [Test]
        public void NotEqualWhenWrongAttributeOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                            "</Dummy>";

            var ex1 = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  The order of atributes is incorrect.\r\n" +
                           "  Xml differ at line 3 index 17.\r\n" +
                           "  Expected: 3| <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                           "  But was:  3| <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                           "  ------------------------------^";

            Assert.AreEqual(expected, ex1.Message);

            var ex2 = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.Verbatim));
            Assert.AreEqual(expected, ex2.Message);
        }

        [Test]
        public void EqualIgnoreAttributeOrder()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value1 Attribute1=\"1\" Attribute2=\"2\">1</Value1>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value1 Attribute2=\"2\" Attribute1=\"1\">1</Value1>\r\n" +
                            "</Dummy>";

            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreAttributeOrder);
            XmlAssert.Equal(expectedXml, actualXml, XmlAssertOptions.IgnoreOrder);
        }

        [Test]
        public void NotEqualWhenWrongNestedElementValue()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer Attribute=\"meh\">\r\n" +
                              "    <Value Attribute=\"1\">2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer Attribute=\"meh\">\r\n" +
                            "    <Value Attribute=\"1\">Wrong</Value>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 4 index 21.\r\n" +
                           "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                           "  But was:  4| <Value Attribute=\"1\">Wrong</Value>\r\n" +
                           "  ----------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongNestedAttribute()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer Attribute=\"meh\">\r\n" +
                              "    <Value Attribute=\"1\">2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer Attribute=\"meh\">\r\n" +
                            "    <Value Wrong=\"1\">2</Value>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 4 index 7.\r\n" +
                           "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                           "  But was:  4| <Value Wrong=\"1\">2</Value>\r\n" +
                           "  --------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongNestedAttributeValue()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Outer Attribute=\"meh\">\r\n" +
                              "    <Value Attribute=\"1\">2</Value>\r\n" +
                              "  </Outer>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Outer Attribute=\"meh\">\r\n" +
                            "    <Value Attribute=\"Wrong\">2</Value>\r\n" +
                            "  </Outer>\r\n" +
                            "</Dummy>";


            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 4 index 18.\r\n" +
                           "  Expected: 4| <Value Attribute=\"1\">2</Value>\r\n" +
                           "  But was:  4| <Value Attribute=\"Wrong\">2</Value>\r\n" +
                           "  -------------------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }

        [Test]
        public void NotEqualWhenWrongElementValue()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                              "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                              "  <Value>1</Value>\r\n" +
                              "</Dummy>";

            var actualXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                            "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
                            "  <Value>Wrong</Value>\r\n" +
                            "</Dummy>";

            var xmlExt = Assert.Throws<AssertException>(() => XmlAssert.Equal(expectedXml, actualXml));
            var expected = "  Expected and actual xml are not equal.\r\n" +
                           "  Xml differ at line 3 index 7.\r\n" +
                           "  Expected: 3| <Value>1</Value>\r\n" +
                           "  But was:  3| <Value>Wrong</Value>\r\n" +
                           "  --------------------^";
            Assert.AreEqual(expected, xmlExt.Message);
        }
    }
}
