namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    /// <summary>Test serialization using <see cref="XmlSerializer"/></summary>
    public static class XmlSerializerAssert
    {
        /// <summary>
        /// 1. serializes <paramref name="expected"/> and <paramref name="actual"/> to xml strings using <see cref="XmlSerializer"/>
        /// 2. Compares the xml using <see cref="XmlAssert"/>
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void Equal<T>(T expected, T actual)
        {
            var exml = ToXml(expected, nameof(expected));
            var axml = ToXml(actual, nameof(actual));
            XmlAssert.Equal(exml, axml);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an xml string using <see cref="XmlSerializer"/>
        /// 2 Compares the xml with <paramref name="expectedXml"/>
        /// 3 Creates a ContainerClass&lt;T&gt; this is to catch errors in ReadEndElement() when implementing <see cref="IXmlSerializable"/>
        /// 4 Serializes it to xml.
        /// 5 Compares the xml
        /// 6 Deserializes it to container class
        /// 7 Does 2 &amp; 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedXml">The expected xml</param>
        /// <param name="actual">The actual item</param>
        /// <param name="options">How to compare the xml</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(string expectedXml, T actual, XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            return Equal(expectedXml, actual, null, null, options);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an xml string using <see cref="XmlSerializer"/>
        /// 2 Compares the xml with <paramref name="expectedXml"/>
        /// 3 Creates a ContainerClass&lt;T&gt; this is to catch errors in ReadEndElement() when implementing <see cref="IXmlSerializable"/>
        /// 4 Serializes it to xml.
        /// 5 Compares the xml
        /// 6 Deserializes it to container class
        /// 7 Does 2 &amp; 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedXml">The expected xml</param>
        /// <param name="actual">The actual item</param>
        /// <param name="elementComparer">
        /// Custom element comparer
        /// This comparer is used first, then the default comparer is used as fallback.
        /// </param>
        /// <param name="options">How to compare the xml</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(
            string expectedXml,
            T actual,
            IEqualityComparer<XElement> elementComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            return Equal(expectedXml, actual, elementComparer, null, options);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an xml string using <see cref="XmlSerializer"/>
        /// 2 Compares the xml with <paramref name="expectedXml"/>
        /// 3 Creates a ContainerClass&lt;T&gt; this is to catch errors in ReadEndElement() when implementing <see cref="IXmlSerializable"/>
        /// 4 Serializes it to xml.
        /// 5 Compares the xml
        /// 6 Deserializes it to container class
        /// 7 Does 2 &amp; 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedXml">The expected xml</param>
        /// <param name="actual">The actual item</param>
        /// <param name="attributeComparer">
        /// Custom attribute comparer
        /// This comparer is used first, then the default comparer is used as fallback.
        /// </param>
        /// <param name="options">How to compare the xml</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(
            string expectedXml,
            T actual,
            IEqualityComparer<XAttribute> attributeComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            var actualXml = ToXml(actual, nameof(actual));
            XmlAssert.Equal(expectedXml, actualXml, null, attributeComparer, options);
            return Roundtrip(actual);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an xml string using <see cref="XmlSerializer"/>
        /// 2 Compares the xml with <paramref name="expectedXml"/>
        /// 3 Creates a ContainerClass&lt;T&gt; this is to catch errors in ReadEndElement() when implementing <see cref="IXmlSerializable"/>
        /// 4 Serializes it to xml.
        /// 5 Compares the xml
        /// 6 Deserializes it to container class
        /// 7 Does 2 &amp; 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedXml">The expected xml</param>
        /// <param name="actual">The actual item</param>
        /// <param name="elementComparer">
        /// Custom element comparer
        /// This comparer is used first, then the default comparer is used as fallback.
        /// </param>
        /// <param name="attributeComparer">
        /// Custom attribute comparer
        /// This comparer is used first, then the default comparer is used as fallback.
        /// </param>
        /// <param name="options">How to compare the xml</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(
            string expectedXml,
            T actual,
            IEqualityComparer<XElement> elementComparer,
            IEqualityComparer<XAttribute> attributeComparer,
            XmlAssertOptions options = XmlAssertOptions.Verbatim)
        {
            var actualXml = ToXml(actual, nameof(actual));
            XmlAssert.Equal(expectedXml, actualXml, elementComparer, attributeComparer, options);
            return Roundtrip(actual);
        }

        /// <summary>
        /// 1. Places <paramref name="item"/> in a ContainerClass{T} container1
        /// 2. Serializes container1
        /// 3. Deserializes the containerXml to container2 and does FieldAssert.Equal(container1, container2);
        /// 4. Serializes container2
        /// 5. Checks XmlAssert.Equal(containerXml1, containerXml2, XmlAssertOptions.Verbatim);
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item"/></typeparam>
        /// <param name="item">The instance to roundtrip</param>
        /// <returns>The serialized and deserialized instance (container2.Other)</returns>
        public static T Roundtrip<T>(T item)
        {
            Roundtripper.Simple(item, nameof(item), ToXml, FromXml<T>);
            var roundtripped = Roundtripper.InContainer(
                item,
                nameof(item),
                ToXml,
                FromXml<ContainerClass<T>>,
                (e, a) => XmlAssert.Equal(e, a, XmlAssertOptions.Verbatim));

            FieldAssert.Equal(item, roundtripped);

            return roundtripped;
        }

        /// <summary>
        /// 1. Creates an XmlSerializer(typeof(T))
        /// 2. Serialize <paramref name="item"/>
        /// 3. Returns the xml
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="item">The item to serialize</param>
        /// <returns>The xml representation of <paramref name="item>"/></returns>
        public static string ToXml<T>(T item)
        {
            return ToXml(item, nameof(item));
        }

        /// <summary>
        /// Get copy paste friendly xml for <paramref name="item"/>
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item"/></typeparam>
        /// <param name="item">The item to serialize</param>
        /// <returns>Xml escaped and ready to paste in code.</returns>
        public static string ToEscapedXml<T>(T item)
        {
            return ToXml(item).Escape(); // wasteful allocation here but np I think;
        }

        /// <summary>
        /// 1. Creates an XmlSerializer(typeof(T))
        /// 2. Deserialize <paramref name="xml"/>
        /// 3. Returns the deserialized instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="xml">The string containing the xml</param>
        /// <returns>The deserialized instance</returns>
        public static T FromXml<T>(string xml)
        {
            return FromXml<T>(xml, nameof(xml));
        }

        private static T FromXml<T>(string xml, string parameterName)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xml))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException($"Could not deserialize {parameterName} to an instance of type {typeof(T)}", e);
            }
        }

        private static string ToXml<T>(T item, string parameterName)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, item);
                    return writer.ToString();
                }
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException($"Could not serialize{parameterName}.", e);
            }
        }

        // ReSharper disable once UnusedMember.Local hiding object.Equals
        private static new void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }
    }
}