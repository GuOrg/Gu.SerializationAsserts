namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A deep equals checking nested fields.
    /// Handles collections and reference loops.
    /// </summary>
    /// <typeparam name="T">The type of instances to check</typeparam>
    public class FieldComparer<T> : IEqualityComparer<T>, IComparer
    {
        public static readonly FieldComparer<T> Default = new FieldComparer<T>();

        private FieldComparer()
        {
        }

        /// <inheritdoc/>
        public bool Equals(T x, T y)
        {
            var comparison = DeepEqualsNode.CreateFor(x, y);
            return comparison.Matches();
        }

        /// <summary>
        /// nUnit uses IComparer for CollectionAssert
        /// </summary>
        int IComparer.Compare(object x, object y)
        {
            var comparison = DeepEqualsNode.CreateFor(x, y);
            if (comparison.Matches())
            {
                return 0;
            }

            return -1;
        }

        /// <inheritdoc/>
        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
