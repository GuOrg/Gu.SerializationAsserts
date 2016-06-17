namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Gu.State;

    /// <summary>For comparing instances using a deep equals comparing fields.</summary>
    public static class FieldAssert
    {
        /// <summary>
        /// Checks:
        /// - All fields and nested fields.
        /// - All elements of Enumerables.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="x"/> and <paramref name="y"/></typeparam>
        /// <param name="x">The first value</param>
        /// <param name="y">The second value</param>
        public static void Equal<T>(T x, T y)
        {
            var diff = DiffBy.FieldValues(x, y);
            if (diff.IsEmpty)
            {
                return;
            }

            using (var writer = new StringWriter())
            {
                var count = diff.Diffs.Count;
                if (count == 1)
                {
                    writer.WriteLine("  Found this difference between expected and actual:");
                }
                else if (count <= 5)
                {
                    writer.WriteLine($"  Fields differ between expected and actual, here are the {count} differences:");
                }
                else if (count > 5)
                {
                    writer.WriteLine($"  Fields differ between expected and actual, here are the first 5 differences:");
                }

                int i = 0;
                foreach (var subDiff in diff.Diffs)
                {
                    if (i > 4)
                    {
                        break;
                    }

                    if (i != 0)
                    {
                        writer.WriteLine();
                        writer.WriteLine();
                    }

                    writer.Write("  expected");
                    writer.WritePath(subDiff, d => d.X);
                    writer.WriteLine();

                    writer.Write("    actual");
                    writer.WritePath(subDiff, d => d.Y);
                    i++;
                }

                var message = writer.ToString();
                throw new AssertException(message);
            }
        }

        // Using new here to hide it so it not called by mistake
        // ReSharper disable once UnusedMember.Local
        private static new void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }

        private static StringWriter WritePath(this StringWriter writer, SubDiff diff, Func<SubDiff, object> valueGetter)
        {
            var fieldDiff = diff as FieldDiff;
            if (fieldDiff != null)
            {
                writer.Write(".");
                writer.Write(fieldDiff.FieldInfo.Name);
            }

            var indexDiff = diff as IndexDiff;
            if (indexDiff != null)
            {
                writer.Write("[");
                writer.Write(indexDiff.Index);
                writer.Write("]");
            }

            if (diff.Diffs.Count == 0)
            {
                writer.Write(": ");
                writer.Write(valueGetter(diff) ?? "null");
                return writer;
            }

            return writer.WritePath(diff.Diffs.First(), valueGetter);
        }
    }
}