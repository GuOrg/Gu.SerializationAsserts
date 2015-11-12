# Gu.SerializationAsserts

## XmlAssert
    XmlAssert.Equal(expectedXml, actualXml);

Compares two xml strings. If they are not equal an `AssertException` is thrown. The exception has a message that can give hints about what is wrong, sample:
```
  String lengths are both 77.
  Strings differ at line 2 index 1.
  Expected: 2| <Expected>
  But was:  1| <Actual>
  --------------^
```
##### XmlAssertOptions
Use `XmlAssertOptions` to compare how the the documents are compared. Sample:

    XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreDeclaration | XmlAssertOptions.IgnoreNameSpaces);

- Verbatim, the default and strictest mode.
- IgnoreDeclaration, ignore Xml declaration when comparing.
- IgnoreNameSpaces, perhaps useful for less verbose tests.
- IgnoreElementOrder
- IgnoreAttributeOrder
- IgnoreOrder = IgnoreElementOrder | IgnoreAttributeOrder

## XmlSerializerAssert
A collection of helpers for tests using `XmlSerializer`. The most typical use is probably:

##### Roundtrip
    var actual = new Dummy { Value = 2 };
    XmlSerializerAssert.RoundTrip(actual);

`T RoundTrip<T>(T item)`
1. Serializes actual to xml
2. Deserializes xml 
3. Compares the instances using `FieldAsser.Equal`
4. Creates a `ContainerClass<T>` and roundtrips it. The reason we do this is to catch errors with ReadEndElement or reading outside the end of the element.
5. Returns the roundtripped instance in case you want to do additional asserts.

##### Equal
    var expectedXml = "<Dummy>\r\n" +
                      "  <Value>2</Value>\r\n" +
                      "</Dummy>";
    var actual = new Dummy { Value = 2 };
    XmlSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreDeclaration | XmlAssertOptions.IgnoreNameSpaces);

1. Serializes actual to xml
2. Calls XmlAssert.Equal(expectedXml, actual, options)
3. Calls XmlSerializerAssert.Roundtrip(actual)
4. Returns the roundtripped instance in case you want to do additional asserts.

<!>

    var expected = new Dummy { Value = 2 };
    var actual = new Dummy { Value = 2 };
    XmlSerializerAssert.Equal(expected, actual);
Checks if two instances produces the same xml. Useful for deep equals.

1. Serializes expected and actual
2. Calls XmlAssert.Equal(expectedXml, actualXml)
