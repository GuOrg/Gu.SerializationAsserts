namespace Gu.SerializationAsserts
{
    public static class AssertSerialization
    {
        public static RoundtripResults<T> RoundtripAll<T>(T item)
        {
            var binary = BinaryFormatterAssert.Roundtrip(item);
            var xmlSerializer = XmlSerializerAssert.RoundTrip(item);
            var dataContract = DataContractSerializerAssert.RoundTrip(item);
            return new RoundtripResults<T>(binary, xmlSerializer, dataContract);
        }
    }
}