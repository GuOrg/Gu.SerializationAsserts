using System;

namespace Gu.SerializationAsserts
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    ///
    /// </summary>
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
            var exml = ToXml(expected);
            var axml = ToXml(actual);
            XmlAssert.Equal(exml, axml);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an xml string using <see cref="XmlSerializer"/>
        /// 2 Compares the xml with <paramref name="expectedXml"/>
        /// 3 Creates a ContainerClass{T} this is to catch errors in ReadEndElement() when implementing <see cref="IXmlSerializable"/>
        /// 4 Serializes it to xml.
        /// 5 Compares the xml
        /// 6 Deserializes it to container class
        /// 7 Does 2 & 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedXml">The expected xml</param>
        /// <param name="actual">The actual item</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(string expectedXml, T actual)
        {
            var actualXml = ToXml(actual);
            XmlAssert.Equal(expectedXml, actualXml);

            var container = new ContainerClass<T>(actual);
            var expectedContainerXml = container.CreateExpectedXmlFor(actualXml);

            for (int i = 0; i < 2; i++)
            {
                var actualContainerXml = ToXml(container);
                XmlAssert.Equal(expectedContainerXml, actualContainerXml);
                container = FromXml<ContainerClass<T>>(actualContainerXml);
            }

            return container.Other;
        }

        /// <summary>
        /// 1. Creates an XmlSerializer(typeof(T))
        /// 2. Serialize <paramref name="item"/>
        /// 3. Returns the xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns>The xml representation of <paramref name="item>"/></returns>
        public static string ToXml<T>(T item)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, item);
                return writer.ToString();
            }
        }

        public static string ToEscapedXml<T>(T item)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, item);
                return writer.ToString().Escape(); // wasteful allocation here but np I think
            }
        }

        /// <summary>
        /// 1. Creates an XmlSerializer(typeof(T))
        /// 2. Deserialize <paramref name="xml"/>
        /// 3. Returns the deserialized instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns>The deserialized instance</returns>
        public static T FromXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }
    }
}