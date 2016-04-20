namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>A comparer with a collection of comparers.</summary>
    public class XElementMultiComparer : IEqualityComparer<XElement>
    {
        private readonly IReadOnlyList<IEqualityComparer<XElement>> comparers;

        public XElementMultiComparer(params IEqualityComparer<XElement>[] comparers)
        {
            this.comparers = comparers;
        }

        public XElementMultiComparer(IEnumerable<IEqualityComparer<XElement>> comparers)
        {
            this.comparers = comparers.ToList();
        }

        /// <summary>Calls Equals for all inner comparers.</summary>
        /// <returns>true if any comparer returns true.</returns>
        public bool Equals(XElement x, XElement y)
        {
            return this.comparers.Any(c => c.Equals(x, y));
        }

        int IEqualityComparer<XElement>.GetHashCode(XElement obj)
        {
            throw new NotSupportedException();
        }
    }
}