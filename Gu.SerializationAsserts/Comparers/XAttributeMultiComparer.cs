namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>A comparer with a collection of comparers.</summary>
    public class XAttributeMultiComparer : IEqualityComparer<XAttribute>
    {
        private readonly IReadOnlyList<IEqualityComparer<XAttribute>> comparers;

        /// <summary>Initializes a new instance of the <see cref="XAttributeMultiComparer"/> class.</summary>
        /// <param name="comparers">The collection of comparers.</param>
        public XAttributeMultiComparer(params IEqualityComparer<XAttribute>[] comparers)
        {
            this.comparers = comparers;
        }

        /// <summary>Initializes a new instance of the <see cref="XAttributeMultiComparer"/> class.</summary>
        /// <param name="comparers">The collection of comparers.</param>
        public XAttributeMultiComparer(IEnumerable<IEqualityComparer<XAttribute>> comparers)
        {
            this.comparers = comparers.ToList();
        }

        /// <summary>Returns true if any comparer returns true.</summary>
        /// <returns>true if any comparer returns true</returns>
        public bool Equals(XAttribute x, XAttribute y)
        {
            return this.comparers.Any(c => c.Equals(x, y));
        }

        /// <summary>Not supported.</summary>
        int IEqualityComparer<XAttribute>.GetHashCode(XAttribute obj)
        {
            throw new NotSupportedException();
        }
    }
}