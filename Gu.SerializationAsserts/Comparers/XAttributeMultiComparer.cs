namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class XAttributeMultiComparer : IEqualityComparer<XAttribute>
    {
        private readonly IReadOnlyList<IEqualityComparer<XAttribute>> comparers;

        public XAttributeMultiComparer(params IEqualityComparer<XAttribute>[] comparers)
        {
            this.comparers = comparers;
        }

        public XAttributeMultiComparer(IEnumerable<IEqualityComparer<XAttribute>> comparers)
        {
            this.comparers = comparers.ToList();
        }

        public bool Equals(XAttribute x, XAttribute y)
        {
            return this.comparers.Any(c => c.Equals(x, y));
        }

        public int GetHashCode(XAttribute obj)
        {
            throw new NotSupportedException();
        }
    }
}