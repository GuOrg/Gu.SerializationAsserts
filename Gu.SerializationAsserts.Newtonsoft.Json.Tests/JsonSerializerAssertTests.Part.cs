namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests
{
    using Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos;

    using NUnit.Framework;

    public partial class JsonSerializerAssertTests
    {

        [Test]
        public void RoundtripWithArrayOfInts()
        {
            var ints = new[] { 1, 2, 3 };
            var actual = new WithIntArray { Values = ints };
            var expectedJson = "{\"Values\":[1,2,3]}";
            var roundtrips = new[]
                                 {
                                         JsonSerializerAssert.Equal(expectedJson, actual),
                                         JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Default),
                                         JsonSerializerAssert.Equal(expectedJson, actual, JsonAssertOptions.Verbatim),
                                         JsonSerializerAssert.Roundtrip(actual)
                                     };
            foreach (var roundtrip in roundtrips)
            {
                CollectionAssert.AreEqual(ints, roundtrip.Values);
            }
        }
    }
}