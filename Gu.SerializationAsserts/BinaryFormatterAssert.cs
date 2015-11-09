namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml.Serialization;

    public static class BinaryFormatterAssert
    {
        /// <summary>
        /// 1 Creates a ContainerClass{T} this is to catch errors in ReadEndElement when implementing IXmlSerilizable
        /// 2 Writes it to a memorystream.
        /// 3 Deserializes it to container class
        /// 4 Does 2 & 3 again
        /// 5 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="expectedXml">The expected xml</param>
        /// <returns></returns>
        public static T Roundtrip<T>(T item, string expectedXml)
        {
            var actual = ToStream(item);
            XmlAssert.AreEqual(expectedXml, actual);

            var container = new ContainerClass<T>(item);
            var expectedContainerXml = container.CreateExpectedXmlFor(actual);

            for (int i = 0; i < 2; i++)
            {
                var actualContainerXml = ToStream(container);
                XmlAssert.AreEqual(expectedContainerXml, actualContainerXml);
                container = FromStream<ContainerClass<T>>(actualContainerXml);
            }

            return container.Other;
        }

        private static MemoryStream ToStream<T>(T item)
        {
            using (var stream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, item);
                stream.Position = 0;
                return stream;
            }
        }

        private static T FromStream<T>(MemoryStream stream)
        {
            var binaryFormatter = new BinaryFormatter();
            var value = (T)binaryFormatter.Deserialize(stream);
            return value;
        }
    }
}