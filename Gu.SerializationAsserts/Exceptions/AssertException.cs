namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    [Serializable]
    public class AssertException : Exception
    {
        private static readonly string TabString = "  ";

        public AssertException(string message)
            : base(message)
        {
        }

        public AssertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static AssertException CreateFromException(string message, Exception innerException)
        {
            using (var writer = new IndentedTextWriter(new StringWriter(), TabString))
            {
                writer.Indent++;
                writer.Write(TabString); // dunno why it fails on the first line
                writer.WriteLine(message);
                writer.WriteMessages(innerException);
                var exceptionMessages = writer.InnerWriter.ToString();
                return new AssertException(exceptionMessages, innerException);
            }
        }
    }
}