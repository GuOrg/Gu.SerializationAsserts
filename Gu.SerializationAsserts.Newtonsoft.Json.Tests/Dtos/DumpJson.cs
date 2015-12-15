namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos
{
    using System;
    using global::Newtonsoft.Json;
    using NUnit.Framework;

    public class DumpJson
    {
        [Test, Explicit]
        public void Dump()
        {
            var item = new Dummy(1);
            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var json = JsonSerializerAssert.ToJson(item, settings);
            Console.Write(json);
        }
    }
}
