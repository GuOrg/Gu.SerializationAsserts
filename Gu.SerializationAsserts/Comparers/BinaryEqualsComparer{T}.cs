namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public class BinaryEqualsComparer<T> : IEqualityComparer<T>, IComparer
    {
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

            var xs = this.ToBytes(x);
            var ys = this.ToBytes(y);
            if (xs.Length != ys.Length)
            {
                return false;
            }

            for (int i = 0; i < xs.Length; i++)
            {
                if (xs[i] != ys[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(T obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            var bytes = this.ToBytes(obj);
            unchecked
            {
                var result = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    result = (result * 31) ^ bytes[i];
                }

                return result;
            }
        }

        int IComparer.Compare(object x, object y)
        {
            return this.Equals((T)x, (T)y) ? 0 : 1;
        }

        private byte[] ToBytes(object o)
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
