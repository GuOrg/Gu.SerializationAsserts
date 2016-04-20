using Gu.SerializationAsserts.Tests.Dtos;

namespace Gu.SerializationAsserts.Tests.Internals
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;

    using NUnit.Framework;

    public class StringExtTests
    {
        [TestCase("abc")]
        [TestCase(@"<?xml version=""1.0"" encoding=""utf-16""?>")]
        //[TestCase("a\r\nb")]
        public void Escape(string text)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(text), writer, null);
                    var code = writer.ToString();
                    Console.WriteLine(code);
                    var escaped = text.Escape();
                    Console.WriteLine(escaped);
                    Assert.AreEqual(code, escaped);
                }
            }
        }

        [Test]
        public void EscapeMultiline()
        {
            var text = "a\r\nb";
            var escaped = text.Escape();
            Console.WriteLine(escaped);
            Assert.AreEqual("\"a\\r\\n\" +\r\n\"b\"", escaped);
        }

        [Test]
        public void TestName()
        {
            ////var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
            ////          "<Dummy xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n" +
            ////          "  <Value>2</Value>\r\n" +
            ////          "</Dummy>";
            var dummy = new Dummy(2);
            var escape = XmlSerializerAssert.ToXml(dummy).Escape();
            Console.WriteLine(escape);
        }
    }
}