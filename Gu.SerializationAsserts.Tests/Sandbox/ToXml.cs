namespace Gu.SerializationAsserts.Tests.Sandbox
{
    using System;
    using System.Collections.Generic;

    using Gu.SerializationAsserts.Tests.Dtos;

    using NUnit.Framework;

    public class ToXml
    {
        [Test]
        public void List()
        {
            var dummies = new List<Dummy> { new Dummy(1), new Dummy(1) };
            var xml = XmlSerializerAssert.ToXml(dummies)
                                               .Escape();
            Console.Write(xml);
        }
    }
}
