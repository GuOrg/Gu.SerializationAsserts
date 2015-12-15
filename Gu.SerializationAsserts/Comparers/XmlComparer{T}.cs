namespace Gu.SerializationAsserts
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Serializes the comparands using <see cref="System.Xml.Serialization.XmlSerializer"/> and compares the bytes.
    /// </summary>
    /// <typeparam name="T">The type of the items to compare</typeparam>
    public class XmlComparer<T> : IEqualityComparer<T>, IComparer
    {
        public static readonly XmlComparer<T> Default = new XmlComparer<T>();

        private XmlComparer()
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

            var xXml = XmlSerializerAssert.ToXml(x);
            var yXml = XmlSerializerAssert.ToXml(y);
            return xXml == yXml;
        }

        /// <inheritdoc/>
        public int GetHashCode(T obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            var xml = XmlSerializerAssert.ToXml(obj);
            return xml.GetHashCode();
        }

        /// <inheritdoc/>
        int IComparer.Compare(object x, object y)
        {
            return this.Equals((T)x, (T)y) ? 0 : 1;
        }
    }
}