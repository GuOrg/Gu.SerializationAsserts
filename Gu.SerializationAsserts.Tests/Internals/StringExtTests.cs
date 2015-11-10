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
    }
}
