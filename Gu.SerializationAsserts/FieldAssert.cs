using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gu.SerializationAsserts
{
    using System;

    public static class FieldAssert
    {
        /// <summary>
        /// Checks:
        /// - All fields and nested fields.
        /// - All elements of IEnumerable fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void Equal<T>(T x, T y)
        {
            var comparison = Comparison.CreateFor(x, y);
            if (comparison.Matches())
            {
                return;
            }

            using (var writer = new StringWriter())
            {
                var allChildren = comparison.AllChildren().ToArray();
                var leafs = allChildren
                    .Where(c => !c.GetChildren().Any() && !c.Matches())
                    .First();
                writer.WriteLine("  Fields differ first diff:");
                var path = GetPath(leafs);
                writer.Write("  expected");
                WritePath(writer, path);
                writer.Write($": {leafs.Expected.Value?.ToString() ?? "null"}");
                writer.WriteLine();
                writer.Write("    actual");
                WritePath(writer, path);
                writer.Write($": {leafs.Actual.Value?.ToString() ?? "null"}");
                var message = writer.ToString();
                throw new AssertException(message);
            }
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }

        private static IReadOnlyList<ICompared> GetPath(this Comparison comparison)
        {
            var path = new List<ICompared>();
            while (comparison.ParentField != null)
            {
                path.Add(comparison.Expected);
                comparison = comparison.Parent;
            }

            path.Reverse();
            return path;
        }

        private static StringWriter WritePath(this StringWriter writer, IReadOnlyList<ICompared> path)
        {
            foreach (var compared in path)
            {
                var field = compared as ComparedField;
                if (field != null)
                {
                    writer.Write($".{field.ParentField.Name}");
                    continue;
                }

                var item = compared as ComparedItem;
                if (item != null)
                {
                    writer.Write($"[{item.Index}]");
                    continue;
                }

                throw new InvalidOperationException();
            }

            return writer;
        }
    }
}