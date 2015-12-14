namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A deep equals checking nested fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldComparer<T> : IEqualityComparer<T>, IComparer
    {
        public static readonly FieldComparer<T> Default = new FieldComparer<T>();

        private FieldComparer()
        {
        }

        public bool Equals(T x, T y)
        {
            var comparison = DeepEqualsNode.CreateFor(x, y);
            return comparison.Matches();
        }

        /// <summary>
        /// nUnit uses IComparer for CollectionAssert
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int IComparer.Compare(object x, object y)
        {
            var comparison = DeepEqualsNode.CreateFor(x, y);
            if (comparison.Matches())
            {
                return 0;
            }

            return -1;
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
