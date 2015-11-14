namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;

    public class XmlAssertOptionsComparer : IEqualityComparer<XmlAssertOptions>
    {
        public static readonly XmlAssertOptionsComparer Default = new XmlAssertOptionsComparer();

        private XmlAssertOptionsComparer()
        {
        }

        public bool Equals(XmlAssertOptions x, XmlAssertOptions y)
        {
            return x == y;
        }

        public int GetHashCode(XmlAssertOptions obj)
        {
            return (int)obj;
        }
    }
}