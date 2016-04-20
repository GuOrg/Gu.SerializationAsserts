# Gu.SerializationAsserts
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![NuGet](https://img.shields.io/nuget/v/Gu.SerializationAsserts.svg)](https://www.nuget.org/packages/Gu.SerializationAsserts/)
[![Build status](https://ci.appveyor.com/api/projects/status/9vyu94ma5vy25ueo?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-serializationasserts)

## Table of Contents

- [1. XmlAssert](#1-xmlassert)
- [2. XmlSerializerAssert](#2-xmlserializerassert)
- [2.1 RoundTrip](#21-roundtrip)
- [2.2 Equal](#22-equal)
- [3. XmlAssertOptions](#3-xmlassertoptions)
- [The options are:](#-the options are:)
- [4. DataContractSerializerAssert](#4-datacontractserializerassert)
- [5. BinaryFormatterAssert](#5-binaryformatterassert)
- [6. FieldAssert](#6-fieldassert)

## 1. XmlAssert
Class exposing method for comparing xml strings.

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

## 2. XmlSerializerAssert
A collection of helpers for tests using `XmlSerializer`. 

To test that an instance can be roundtripped use:

### 2.1 RoundTrip
1. Serializes actual to xml
2. Deserializes xml 
3. Compares the instances using `FieldAsser.Equal`
4. Creates a `ContainerClass<T>` and roundtrips it. The reason we do this is to catch errors with ReadEndElement or reading outside the end of the element.
5. Returns the roundtripped instance in case you want to do additional asserts.

```c#
var actual = new Dummy { Value = 2 };
XmlSerializerAssert.RoundTrip(actual);
```

### 2.2 Equal

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

## 3. XmlAssertOptions
Use `XmlAssertOptions` to specify how the the xml is compared. 
Sample:

```c#
XmlAssert.Equal(expected, actual, XmlAssertOptions.IgnoreDeclaration | XmlAssertOptions.IgnoreNamespaces);
```
### The options are:
- Verbatim, the default and strictest mode.
- IgnoreDeclaration, ignore Xml declaration when comparing.
- IgnoreNamespaces, perhaps useful for less verbose tests.
- IgnoreElementOrder
- IgnoreAttributeOrder
- IgnoreOrder = IgnoreElementOrder | IgnoreAttributeOrder

## 4. DataContractSerializerAssert 
Similar to XmlSerializerAssert

`HasDataContractAttribute<T>()` checks that [DataContract] is defined for T

`AllPropertiesHasDataMemberAttributes<T>()` checks that all properties for T has [DataMember]

## 5. BinaryFormatterAssert 
Similar to XmlSerializerAssert

## 6. FieldAssert
Checks that all fields are structurally equal recursively. Compares `IEquatable<T>` types using `object.Equals()`.
Compares collections by element.

If assert fails the output looks like this:
```
Found this difference between expected and actual:
expected[1].value: 5
  actual[1].value: 2
```
