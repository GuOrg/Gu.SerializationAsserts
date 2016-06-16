namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;

    using Gu.State;

    /// <summary>
    /// A deep equals checking nested fields.
    /// Handles collections and reference loops.
    /// </summary>
    /// <typeparam name="T">The type of instances to check</typeparam>
    public class FieldComparer<T> : IEqualityComparer<T>, IComparer
    {
        /// <summary>The default instance.</summary>
        public static readonly FieldComparer<T> Default = new FieldComparer<T>();

        private FieldComparer()
        {
        }

        /// <inheritdoc/>
        public bool Equals(T x, T y)
        {
            return EqualBy.FieldValues(x, y);
        }

        /// <summary>
        /// nUnit uses IComparer for CollectionAssert
        /// Note: this is not a comparer that makes sense for sorting.
        /// </summary>
        /// <returns>
        /// 0 if <paramref name="x"/> and <paramref name="y"/> are equal.
        /// -1 if not equal.
        /// </returns>
        int IComparer.Compare(object x, object y)
        {
            return EqualBy.FieldValues(x, y)
                       ? 0
                       : -1;
        }

        /// <inheritdoc/>
        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
