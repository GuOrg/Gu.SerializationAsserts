namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <summary>A comparer that trims the value of <see cref="XElement.Value"/> before comparing.</summary>
    public class XElementTrimValueComparer : IEqualityComparer<XElement>
    {
        public static readonly XElementTrimValueComparer Default = new XElementTrimValueComparer();

        private XElementTrimValueComparer()
        {
        }

        /// <summary>return x.Value.Trim() == y.Value.Trim();</summary>
        /// <returns>true if the trimmed values are equal</returns>
        public bool Equals(XElement x, XElement y)
        {
            return x.Value.Trim() == y.Value.Trim();
        }

        /// <inheritdoc/>
        public int GetHashCode(XElement obj)
        {
            return obj?.Value.Trim()
                      .GetHashCode() ?? 0;
        }
    }
}