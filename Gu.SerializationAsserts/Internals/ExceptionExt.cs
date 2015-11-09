namespace Gu.SerializationAsserts
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    public static class ExceptionExt
    {
        /// <summary>
        /// Writes all InnerException messages indented as a string.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetNestedMessages(this Exception e)
        {
            using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
            {
                WriteMessages(e, writer);
                return writer.InnerWriter.ToString();
            }
        }

        private static void WriteMessages(Exception e, IndentedTextWriter writer)
        {
            writer.WriteLine(e.GetType().Name);
            writer.Write(e.Message);
            Console.WriteLine();
            Console.WriteLine();
            if (e.InnerException != null)
            {
                writer.Indent++;
                WriteMessages(e.InnerException, writer);
            }
        }
    }
}
