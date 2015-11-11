namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// A deep equals checking nested fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FieldsEqualsComparer<T> : IEqualityComparer<T>, IComparer
    {
        public static readonly FieldsEqualsComparer<T> Default = new FieldsEqualsComparer<T>();

        private FieldsEqualsComparer()
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

            return 1;
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
