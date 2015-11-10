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
        /// 1 Serializes <paramref name="item"/> to an xml string using <see cref="XmlSerializer"/>
        /// 2 Compares the xml with <paramref name="expectedXml"/>
        /// 3 Creates a ContainerClass{T} this is to catch errors in ReadEndElement when implementing IXmlSerilizable
        /// 4 Serializes it to xml.
        /// 5 Compares the xml
        /// 6 Deserializes it to container class
        /// 7 Does 2 & 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="expectedXml">The expected xml</param>
        /// <returns></returns>
        public static T Roundtrip<T>(T item, string expectedXml)
        {
            var actual = ToXml(item);
            XmlAssert.AreEqual(expectedXml, actual);

            var container = new ContainerClass<T>(item);
            var expectedContainerXml = container.CreateExpectedXmlFor(actual);

            for (int i = 0; i < 2; i++)
            {
                var actualContainerXml = ToXml(container);
                XmlAssert.AreEqual(expectedContainerXml, actualContainerXml);
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
    }
}