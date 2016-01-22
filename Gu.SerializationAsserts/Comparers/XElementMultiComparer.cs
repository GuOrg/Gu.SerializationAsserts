namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

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

        public bool Equals(XElement x, XElement y)
        {
            return this.comparers.Any(c => c.Equals(x, y));
        }

        public int GetHashCode(XElement obj)
        {
            throw new NotSupportedException();
        }
    }
}