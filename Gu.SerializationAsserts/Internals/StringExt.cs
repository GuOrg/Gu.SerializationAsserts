namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    internal static class StringExt
    {
        private static readonly IReadOnlyDictionary<char, string> Escapes = new Dictionary<char, string>
                                                                                {
                                                                                    { '\\', "\\\\" },
                                                                                    { '\"', "\\\"" },
                                                                                };

        /// <summary>
        /// http://stackoverflow.com/a/324812/1069200
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        internal static string Escape(this string input)
        {
            using (var writer = new StringWriter())
            {
                writer.Write("\"");
                for (int i = 0; i < input.Length; i++)
                {
                    var c = input[i];
                    string escaped;
                    if (Escapes.TryGetValue(c, out escaped))
                    {
                        writer.Write(escaped);
                        continue;
                    }

                    if (c == '\r' && input.Length > i + 1 && input[i + 1] == '\n')
                    {
                        writer.Write("\\r\\n\" +");
                        writer.WriteLine();
                        i++;
                        if (input.Length > i)
                        {
                            writer.Write("\"");
                        }

                        continue;
                    }

                    writer.Write(c);
                }

                writer.Write("\"");
                return writer.ToString();
            }
        }

        internal static int FirstDiff(this string expectedRow, string actualRow)
        {
            for (int i = 0; i < expectedRow.Length; i++)
            {
                if (actualRow.Length <= i)
                {
                    return i;
                }

                if (expectedRow[i] != actualRow[i])
                {
                    return i;
                }
            }

            return expectedRow.Length;
        }

        internal static string Line(this string text, int lineNumber)
        {
            Ensure.GreaterThanOrEqual(lineNumber, 1, nameof(lineNumber));

            var rows = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lineNumber <= rows.Length)
            {
                return rows[lineNumber - 1];
            }

            return "";
        }

        internal static string[] Lines(this string text, string splitter = null, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
        {
            splitter = splitter ?? Environment.NewLine;
            return text.Split(new[] { splitter }, stringSplitOptions);
        }
    }
}
