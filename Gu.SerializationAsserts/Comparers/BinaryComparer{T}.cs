namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Serializes the comparands using <see cref="BinaryFormatter"/> and compares the bytes.
    /// </summary>
    /// <typeparam name="T">The type of the items to compare</typeparam>
    public class BinaryComparer<T> : IEqualityComparer<T>, IComparer
    {
        /// <summary>The default instance.</summary>
        public static readonly BinaryComparer<T> Default = new BinaryComparer<T>();

        private BinaryComparer()
        {
        }

        /// <inheritdoc/>
        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            var xs = ToBytes(x);
            var ys = ToBytes(y);
            if (xs.Length != ys.Length)
            {
                return false;
            }

            for (var i = 0; i < xs.Length; i++)
            {
                if (xs[i] != ys[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public int GetHashCode(T obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            var bytes = ToBytes(obj);
            unchecked
            {
                var result = 0;
                for (var i = 0; i < bytes.Length; i++)
                {
                    result = (result * 31) ^ bytes[i];
                }

                return result;
            }
        }

        /// <summary>
        /// nUnit uses IComparer for CollectionAssert
        /// Note: this is not a comparer that makes sense for sorting.
        /// </summary>
        /// <param name="x">x value.</param>
        /// <param name="y">y value.</param>
        /// <returns>
        /// 0 if <paramref name="x"/> and <paramref name="y"/> are equal.
        /// -1 if not equal.
        /// </returns>
        int IComparer.Compare(object x, object y)
        {
            return this.Equals((T)x, (T)y) ? 0 : 1;
        }

        private static byte[] ToBytes(object o)
        {
            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, o);
                return stream.ToArray();
            }
        }
    }
}
