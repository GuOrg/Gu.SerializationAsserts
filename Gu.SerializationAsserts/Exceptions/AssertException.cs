namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    /// <summary>An exception thrown if assert fails.</summary>
    [Serializable]
    public class AssertException : Exception
    {
        private static readonly string TabString = "  ";

        /// <summary>Initializes a new instance of the <see cref="AssertException"/> class.</summary>
        /// <param name="message">The message.</param>
        public AssertException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="AssertException"/> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public AssertException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>Creates an <see cref="AssertException"/> with <paramref name="innerException"/> as inner exception.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>An <see cref="AssertException"/></returns>
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