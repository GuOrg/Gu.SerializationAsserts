namespace Gu.SerializationAsserts
{
    using System;

    public static class AssertSerialization
    {
        public static T[] RoundtripAll<T>(T item, bool assertAreEqual = true)
        {
            throw new NotImplementedException("");
            //return new[]
            //           {
            //               BinaryFormatterRoundtrip(item, assertAreEqual),
            //               XmlSerializerRoundtrip(item,null, assertAreEqual),
            //               DataContractSerializerRoundtrip(item, assertAreEqual),
            //           };
        }
    }
}