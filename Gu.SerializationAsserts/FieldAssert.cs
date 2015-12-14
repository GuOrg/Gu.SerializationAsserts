namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class FieldAssert
    {
        /// <summary>
        /// Checks:
        /// - All fields and nested fields.
        /// - All elements of IEnumerable fields.
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="x"/> and <paramref name="y"/></typeparam>
        /// <param name="x">The first value</param>
        /// <param name="y">The second value</param>
        public static void Equal<T>(T x, T y)
        {
            var comparison = DeepEqualsNode.CreateFor(x, y);
            if (comparison.Matches())
            {
                return;
            }

            using (var writer = new StringWriter())
            {
                var children = comparison.AllChildren()
                                      .Where(c => !c.Children.Any() && !c.Matches())
                                      .Take(6)
                                      .ToArray();
                if (children.Length == 1)
                {
                    writer.WriteLine("  Found this difference between expected and actual:");
                }
                else if (children.Length <= 5)
                {
                    writer.WriteLine($"  Fields differ between expected and actual, here are the {children.Length} differences:");
                }
                else if (children.Length > 5)
                {
                    writer.WriteLine($"  Fields differ between expected and actual, here are the first 5 differences:");
                }

                var lastIndex = Math.Min(5, children.Length);
                for (int i = 0; i < lastIndex; i++)
                {
                    if (i != 0)
                    {
                        writer.WriteLine();
                        writer.WriteLine();
                    }

                    var leaf = children[i];
                    var path = GetPath(leaf);
                    writer.Write("  expected");
                    WritePath(writer, path, leaf.Expected.Value);

                    writer.WriteLine();
                    writer.Write("    actual");
                    WritePath(writer, path, leaf.Actual.Value);
                }

                var message = writer.ToString();
                throw new AssertException(message);
            }
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }

        private static IReadOnlyList<ICompared> GetPath(this DeepEqualsNode deepEqualsNode)
        {
            var path = new List<ICompared>();
            while (deepEqualsNode != null)
            {
                path.Add(deepEqualsNode.Expected);
                deepEqualsNode = deepEqualsNode.Parent;
            }

            path.Reverse();
            return path;
        }

        private static StringWriter WritePath(this StringWriter writer, IReadOnlyList<ICompared> path, object value)
        {
            foreach (var compared in path)
            {
                var field = compared as ComparedField;
                if (field != null)
                {
                    if (field.ParentField != null)
                    {
                        writer.Write($".{field.ParentField.Name}");
                    }

                    continue;
                }

                var item = compared as ComparedItem;
                if (item != null)
                {
                    writer.Write($"[{item.Index}]");
                    continue;
                }

                var enumerable = compared as ComparedIEnumerable;
                if (enumerable != null)
                {
                    writer.Write($".Count: {((IEnumerable)value).OfType<object>().Count()}");
                    continue;
                }

                throw new InvalidOperationException();
            }

            if (!(path.Last() is ComparedIEnumerable))
            {
                writer.Write($": {value?.ToString() ?? "null"}");
            }

            return writer;
        }
    }
}