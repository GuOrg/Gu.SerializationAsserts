namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>A comparer that does: return x.Value.Trim() == y.Value.Trim();</summary>
    public class XAttributeTrimValueComparer : IEqualityComparer<XAttribute>
    {
        /// <summary>The default isntance.</summary>
        public static readonly XAttributeTrimValueComparer Default = new XAttributeTrimValueComparer();

        private XAttributeTrimValueComparer()
        {
        }

        /// <summary>return x.Value.Trim() == y.Value.Trim();</summary>
        /// <returns>true if the trimmed values are equal</returns>
        public bool Equals(XAttribute x, XAttribute y)
        {
            return x.Value.Trim() == y.Value.Trim();
        }

        /// <inheritdoc/>
        public int GetHashCode(XAttribute obj)
        {
            return obj?.Value.Trim()
                      .GetHashCode() ?? 0;
        }
    }
}