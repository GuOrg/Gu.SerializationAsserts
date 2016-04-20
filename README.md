# Gu.SerializationAsserts
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/Gu.SerializationAsserts.svg)](https://www.nuget.org/packages/Gu.SerializationAsserts/)
[![NuGet](https://img.shields.io/nuget/v/Gu.SerializationAsserts.Newtonsoft.Json.svg)](https://www.nuget.org/packages/Gu.SerializationAsserts.Newtonsoft.Json/)
[![Build status](https://ci.appveyor.com/api/projects/status/9vyu94ma5vy25ueo?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-serializationasserts)

## Contents

  - [1. Xml](#1-xml)
    - [1.1. XmlAssert](#11-xmlassert)
    - [1.1.1. Equal](#111-equal)
    - [1.1.2 With custom comparer](#112-with-custom-comparer)
    - [1.2. XmlSerializerAssert](#12-xmlserializerassert)
      - [1.2.1. RoundTrip](#121-roundtrip)
      - [1.2.2. Equal](#122-equal)
      - [1.1.3. ToXml](#113-toxml)
      - [1.1.4. FromXml](#114-fromxml)
    - [1.2. DataContractSerializerAssert](#12-datacontractserializerassert)
    - [1.2.1 HasDataContractAttribute&lt;T&gt;()](#121-hasdatacontractattribute&lt;t&gt;())
    - [1.2.1 AllPropertiesHasDataMemberAttributes&lt;T&gt;()](#121-allpropertieshasdatamemberattributes&lt;t&gt;())
    - [1.3. XmlAssertOptions](#13-xmlassertoptions)
      - [1.3.1 The options are:](#131-the-options-are)
  - [2. BinaryFormatterAssert](#2-binaryformatterassert)
  - [3. JSON](#3-json)
    - [3.1 JsonAssert](#31-jsonassert)
    - [3.2 JsonSerializerAssert](#32-jsonserializerassert)
  - [4. FieldAssert](#4-fieldassert)
  - [5. FieldComparer&lt;T&gt;](#5-fieldcomparer&lt;t&gt;)

## 1. Xml
### 1.1. XmlAssert
Class exposing method for comparing xml strings.

### 1.1.1. Equal
```c#
XmlAssert.Equal(expectedXml, actualXml);
```
Compares two xml strings. If they are not equal an `AssertException` is thrown. 
The exception has a message that can give hints about what is wrong, sample:

```
  Xml differ at line 4 index 21.
  Expected: 4| <Value Attribute="1">2</Value>
  But was:  4| <Value Attribute="1">Wrong</Value>
  ----------------------------------^
```

### 1.1.2 With custom comparer

```c#
[Test]
public void EqualWithCustomElementComparer()
{
    var expected = "<Foo>" +
                   "  <Bar>1</Bar>" +
                   "</Foo>";

    var actual = "<Foo>" +
                 "  <Bar>  1.0  </Bar>" +
                 "</Foo>";
    XmlAssert.Equal(expected, actual, new ElementDoubleValueComparer(), XmlAssertOptions.IgnoreDeclaration | XmlAssertOptions.IgnoreNamespaces);
}

private class ElementDoubleValueComparer : IEqualityComparer<XElement>
{
    public bool Equals(XElement x, XElement y)
    {
        return double.Parse(x.Value, CultureInfo.InvariantCulture) == double.Parse(y.Value, CultureInfo.InvariantCulture);
    }

    int IEqualityComparer<XElement>.GetHashCode(XElement obj)
    {
        throw new System.NotImplementedException();
    }
}
```

### 1.2. XmlSerializerAssert
A collection of helpers for tests using [XmlSerializer](https://msdn.microsoft.com/en-us/library/system.xml.serialization.xmlserializer(v=vs.110).aspx). 

To test that an instance can be roundtripped use:

#### 1.2.1. RoundTrip
1. Serializes actual to xml
2. Deserializes xml 
3. Compares the instances using `FieldAsser.Equal`
4. Creates a `ContainerClass<T>` and roundtrips it. The reason we do this is to catch errors with ReadEndElement or reading outside the end of the element.
5. Returns the roundtripped instance in case you want to do additional asserts.

```c#
var actual = new Dummy { Value = 2 };
XmlSerializerAssert.RoundTrip(actual);
```

#### 1.2.2. Equal

To assert that serialization produces the expected xml and that the instance can be roundtripped use:
```c#
var actual = new Dummy { Value = 2 };
var expectedXml = "<Dummy>\r\n" +
                  "  <Value>2</Value>\r\n" +
                  "</Dummy>";
var roundtrip = XmlSerializerAssert.Equal(expectedXml, actual, XmlAssertOptions.IgnoreNamespaces | XmlAssertOptions.IgnoreDeclaration);
```

1. Serializes actual to xml
2. Calls XmlAssert.Equal(expectedXml, actual, options)
3. Calls XmlSerializerAssert.Roundtrip(actual)
4. Returns the roundtripped instance in case you want to do additional asserts.

To compare equality of the xml produced when serializing two instances use:

```c#
var expected = new Dummy { Value = 2 };
var actual = new Dummy { Value = 2 };
XmlSerializerAssert.Equal(expected, actual);
```
Checks if two instances produces the same xml. Useful for deep equals.

1. Serializes expected and actual
2. Calls XmlAssert.Equal(expectedXml, actualXml)

#### 1.1.3. ToXml
Serialize an object:
```c#
var dummy = new Dummy { Value = 2 };
string xml = XmlSerializerAssert.ToXml(dummy);
```

#### 1.1.4. FromXml
Deserialize xml to an object:
```c#
var foo = XmlSerializerAssert.FromXml(xml);
```

### 1.2. DataContractSerializerAssert 
Same as XmlSerializerAssert but uses [DataContractSerializer](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.datacontractserializer(v=vs.110).aspx)

### 1.2.1 HasDataContractAttribute&lt;T&gt;()
Checks that T has [DataContract]

### 1.2.1 AllPropertiesHasDataMemberAttributes&lt;T&gt;()
Checks that all properties for T has the [DataMember] attribute.

    
### 1.3. XmlAssertOptions
Use `XmlAssertOptions` to specify how the the xml is compared. 
Sample:

```c#
XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreDeclaration | XmlAssertOptions.IgnoreNamespaces);
```
#### 1.3.1 The options are:
- Verbatim, the default and strictest mode.
- IgnoreDeclaration, ignore Xml declaration when comparing.
- IgnoreNamespaces, perhaps useful for less verbose tests.
- IgnoreElementOrder
- IgnoreAttributeOrder
- IgnoreOrder = IgnoreElementOrder | IgnoreAttributeOrder

## 2. BinaryFormatterAssert 
Same as XmlSerializerAssert but uses [BinaryFormatter](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.formatters.binary.binaryformatter(v=vs.110).aspx)

## 3. JSON

### 3.1 JsonAssert       
Same as XmlAssert but for JSON.
      
### 3.2 JsonSerializerAssert       
Same as XmlSerializerAssert but uses [JsonSerializer](http://www.newtonsoft.com/json/help/html/t_newtonsoft_json_jsonserializer.htm)

## 4. FieldAssert
Checks that all fields are structurally equal recursively. Compares `IEquatable<T>` types using `object.Equals()`.
Compares collections by element.

```c#
var d1 = new Dummy { Value = 1 };
var d2 = new Dummy { Value = 1 };
FieldAssert.Equal(d1, d1);
```

If assert fails the output looks like this:
```
Found this difference between expected and actual:
expected[1].foo[2].bar.baz: 5
  actual[1].foo[2].bar.baz: 2
```

## 5. FieldComparer&lt;T&gt;
NUnit's CollectionAssert takes an IComparer

```c#
CollectionAssert.AreEqual(actual, expected, FieldComparer.For<Foo>());
```
