namespace Gu.SerializationAsserts
{
    public static class SerializationAssert
    {
        public static RoundtripResults<T> RoundtripAll<T>(T item)
        {
            var binary = BinaryFormatterAssert.Roundtrip(item);
            var xmlSerializer = XmlSerializerAssert.Roundtrip(item);
            var dataContract = DataContractSerializerAssert.Roundtrip(item);
            return new RoundtripResults<T>(binary, xmlSerializer, dataContract);
        }
    }
}