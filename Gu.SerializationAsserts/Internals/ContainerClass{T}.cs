namespace Gu.SerializationAsserts
{
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// This is for catching errors with ReadEndElement etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContainerClass<T>
    {
        public ContainerClass(T item)
        {
            this.First = item;
            this.Other = item;
        }

        private ContainerClass()
        {
        }

        public T First { get; set; }

        public T Other { get; set; }

        internal string CreateExpectedXmlFor(string itemXml)
        {
            return CreateExpectedXml(itemXml);
        }

        private static string CreateExpectedXml(string actual)
        {
            using (var writer = new IndentedTextWriter(new StringWriter(), "  "))
            {
                writer.WriteLine(actual.Lines().First());
                writer.WriteLine($@"<ContainerClassOf{typeof(T).Name} xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">");
                writer.Indent++;
                foreach (var element in new[] { "First", "Other" })
                {
                    writer.WriteLine($"<{element}>");
                    for (int i = 2; i < actual.Lines().Length - 1; i++)
                    {
                        var row = actual.Lines()[i];
                        writer.WriteLine(row);
                    }

                    writer.WriteLine($"</{element}>");
                }

                writer.Indent--;
                writer.WriteLine($"</ContainerClassOf{typeof(T).Name}>");
                return writer.InnerWriter.ToString();
            }
        }
    }
}