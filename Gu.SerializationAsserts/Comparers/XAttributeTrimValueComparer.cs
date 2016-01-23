namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class XAttributeTrimValueComparer : IEqualityComparer<XAttribute>
    {
        public static readonly XAttributeTrimValueComparer Default = new XAttributeTrimValueComparer();

        private XAttributeTrimValueComparer()
        {
        }

        public bool Equals(XAttribute x, XAttribute y)
        {
            return x.Value.Trim() == y.Value.Trim();
        }

        public int GetHashCode(XAttribute obj)
        {
            throw new NotSupportedException();
        }
    }
}