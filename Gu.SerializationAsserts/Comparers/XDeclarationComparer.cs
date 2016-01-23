namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class XDeclarationComparer : IEqualityComparer<XDeclaration>
    {
        public static readonly XDeclarationComparer Default = new XDeclarationComparer();

        private XDeclarationComparer()
        {
        }

        public bool Equals(XDeclaration x, XDeclaration y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            // https://www.w3.org/TR/xml/#charencoding case insensitive
            if (!string.Equals(x.Encoding, y.Encoding, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.Equals(x.Version, y.Version))
            {
                return false;
            }

            if (!string.Equals(x.Standalone, y.Standalone))
            {
                return false;
            }

            return true;
        }

        public int GetHashCode(XDeclaration obj)
        {
            throw new NotSupportedException();
        }
    }
}