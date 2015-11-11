namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    [Serializable]
    public class AssertException : Exception
    {
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
            using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
            {
                writer.Indent++;
                writer.WriteLine(message);
                writer.WriteMessages(innerException);
                var exceptionMessages = writer.InnerWriter.ToString();
                return new AssertException(exceptionMessages, innerException);
            }
        }
    }
}