namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class XElementTrimValueComparer : IEqualityComparer<XElement>
    {
        public static readonly XElementTrimValueComparer Default = new XElementTrimValueComparer();

        private XElementTrimValueComparer()
        {
        }

        public bool Equals(XElement x, XElement y)
        {
            return x.Value.Trim() == y.Value.Trim();
        }

        public int GetHashCode(XElement obj)
        {
            throw new NotSupportedException();
        }
    }
}