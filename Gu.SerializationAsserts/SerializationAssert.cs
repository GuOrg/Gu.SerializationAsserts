namespace Gu.SerializationAsserts
{
    /// <summary>
    /// For asserting that serialization works with <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> and <see cref="System.Xml.Serialization.XmlSerializer"/> and <see cref="System.Runtime.Serialization.DataContractSerializer"/>
    /// </summary>
    public static class SerializationAssert
    {
        /// <summary>
        /// Calls:
        /// - BinaryFormatterAssert.Roundtrip(item)
        /// - XmlSerializerAssert.Roundtrip(item)
        /// - DataContractSerializerAssert.Roundtrip(item)
        /// </summary>
        /// <returns>The roundtripped results.</returns>
        public static RoundtripResults<T> RoundtripAll<T>(T item)
        {
            var binary = BinaryFormatterAssert.Roundtrip(item);
            var xmlSerializer = XmlSerializerAssert.Roundtrip(item);
            var dataContract = DataContractSerializerAssert.Roundtrip(item);
            return new RoundtripResults<T>(binary, xmlSerializer, dataContract);
        }
    }
}