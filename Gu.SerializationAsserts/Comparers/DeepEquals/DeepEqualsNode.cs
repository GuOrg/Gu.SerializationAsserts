namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    [DebuggerDisplay("Type: {Type} Field: {ParentField}}")]
    internal class DeepEqualsNode
    {
        private readonly List<ComparedPair> compared;
        private IReadOnlyList<DeepEqualsNode> children;

        private DeepEqualsNode(ICompared expected, ICompared actual, List<ComparedPair> compared)
        {
            this.Expected = expected;
            this.Actual = actual;
            this.compared = compared;
            var expectedType = this.Expected.Value?.GetType();
            var actualType = this.Actual.Value?.GetType();
            this.Type = expectedType == actualType ? expectedType : null;
            this.Fields = this.Type?.GetFields(BindingFlags.Instance |
                                               BindingFlags.Public |
                                               BindingFlags.NonPublic |
                                               BindingFlags.FlattenHierarchy)
                                     ?? new FieldInfo[0];
        }

        private DeepEqualsNode(
            DeepEqualsNode parent,
            FieldInfo parentField,
            ICompared expected,
            ICompared actual,
            List<ComparedPair> compared)
            : this(expected, actual, compared)
        {
            this.Parent = parent;
            this.ParentField = parentField;
        }

        internal ICompared Expected { get; }

        internal ICompared Actual { get; }

        internal Type Type { get; }

        internal DeepEqualsNode Parent { get; }

        internal FieldInfo ParentField { get; }

        internal IReadOnlyList<FieldInfo> Fields { get; }

        internal IReadOnlyList<DeepEqualsNode> Children => this.children ?? (this.children = this.GetChildren().ToList());

        public IEnumerable<DeepEqualsNode> AllChildren()
        {
            foreach (var child in this.Children)
            {
                yield return child;
                foreach (var nested in child.AllChildren())
                {
                    yield return nested;
                }
            }
        }

        internal static DeepEqualsNode CreateFor(object expected, object actual)
        {
            var ec = new ComparedField(expected, null);
            var ac = new ComparedField(actual, null);
            return new DeepEqualsNode(ec, ac, new List<ComparedPair>());
        }

        internal bool Matches()
        {
            var match = this.compared.SingleOrDefault(x => x.HasCompared(this.Expected.Value, this.Actual.Value));
            if (match != null)
            {
                return match.IsEqual;
            }

            var comparedPair = new ComparedPair(this.Expected.Value, this.Actual.Value);
            this.compared.Add(comparedPair);
            if (this.Expected.Value == null && this.Actual.Value == null)
            {
                comparedPair.IsEqual = true;
                return true;
            }

            if (this.Expected.Value == null || this.Actual.Value == null)
            {
                comparedPair.IsEqual = false;
                return false;
            }

            if (this.Expected is ComparedIEnumerable && this.Actual is ComparedIEnumerable)
            {
                comparedPair.IsEqual = false;
                return false;
            }

            if (this.Type.IsEquatable())
            {
                comparedPair.IsEqual = object.Equals(this.Expected.Value, this.Actual.Value);
                return comparedPair.IsEqual;
            }

            if (this.Children.Any(child => !child.Matches()))
            {
                comparedPair.IsEqual = false;
                return false;
            }

            comparedPair.IsEqual = true;
            return true;
        }

        private static bool IsAnyIEnumerable(object a, object b)
        {
            return a is IEnumerable || b is IEnumerable;
        }

        private static bool IsBothIEnumerable(object a, object b)
        {
            return a is IEnumerable && b is IEnumerable;
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }

        private DeepEqualsNode CreateIEnumerableChild(object[] expected, object[] actual, FieldInfo field)
        {
            var ecf = new ComparedIEnumerable(expected, field);
            var acf = new ComparedIEnumerable(actual, field);
            return new DeepEqualsNode(this, field, ecf, acf, this.compared);
        }

        private DeepEqualsNode CreateFieldChild(object expected, object actual, FieldInfo field)
        {
            var ecf = new ComparedField(expected, field);
            var acf = new ComparedField(actual, field);
            return new DeepEqualsNode(this, field, ecf, acf, this.compared);
        }

        private DeepEqualsNode CreateIndexChild(object expected, object actual, FieldInfo field, int index)
        {
            var ecf = new ComparedItem(expected, index);
            var acf = new ComparedItem(actual, index);
            return new DeepEqualsNode(this, field, ecf, acf, this.compared);
        }

        private IEnumerable<DeepEqualsNode> GetChildren()
        {
            var expected = this.Expected.Value;
            var actual = this.Actual.Value;

            if (expected == null || actual == null)
            {
                yield break;
            }

            if (this.Type != null && this.Type.IsEquatable())
            {
                yield break;
            }

            if (this.Expected is ComparedIEnumerable && this.Actual is ComparedIEnumerable)
            {
                yield break;
            }

            if (IsAnyIEnumerable(expected, actual))
            {
                if (!IsBothIEnumerable(expected, actual))
                {
                    throw new InvalidOperationException("Derp");
                }

                var expectedChildren = ((IEnumerable)expected).OfType<object>().ToArray();
                var actualChildren = ((IEnumerable)actual).OfType<object>().ToArray();
                if (expectedChildren.Length != actualChildren.Length)
                {
                    yield return this.CreateIEnumerableChild(expectedChildren, actualChildren, this.ParentField);
                }
                else
                {
                    for (int i = 0; i < Math.Max(expectedChildren.Length, actualChildren.Length); i++)
                    {
                        var expectedChild = expectedChildren.ElementAtOrDefault(i);
                        var actualChild = actualChildren.ElementAtOrDefault(i);
                        if (this.compared.Any(x => x.HasCompared(expectedChild, actualChild)))
                        {
                            continue;
                        }

                        yield return this.CreateIndexChild(expectedChild, actualChild, this.ParentField, i);
                    }
                }
            }

            foreach (var fieldInfo in this.Fields)
            {
                var expectedChild = fieldInfo.GetValue(expected);
                var actualChild = fieldInfo.GetValue(actual);
                if (this.compared.Any(x => x.HasCompared(expectedChild, actualChild)))
                {
                    continue;
                }

                yield return this.CreateFieldChild(expectedChild, actualChild, fieldInfo);
            }
        }
    }
}