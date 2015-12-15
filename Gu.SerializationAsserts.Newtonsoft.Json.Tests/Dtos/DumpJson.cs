namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos
{
    using System;
    using global::Newtonsoft.Json;
    using NUnit.Framework;

    public class DumpJson
    {
        [Test]
        public void Dump()
        {
            var item = new[] { 1, 2, 3 };
            var json = JsonSerializerAssert.ToEscapedJson(item, new JsonSerializerSettings { Formatting = Formatting.Indented });
            Console.Write(json);
        }
    }
}
