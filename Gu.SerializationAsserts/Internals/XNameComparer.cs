namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class XNameComparer : IEqualityComparer<XName>, IComparer<XName>
    {
        public static readonly XNameComparer IgnoringNameSpaces = new XNameComparer(XmlAssertOptions.IgnoreNameSpaces);
        public static readonly XNameComparer Default = new XNameComparer(XmlAssertOptions.Verbatim);

        private readonly XmlAssertOptions options;

        private XNameComparer(XmlAssertOptions options)
        {
            this.options = options;
        }

        public static XNameComparer GetFor(XmlAssertOptions options)
        {
            if (options.HasFlag(XmlAssertOptions.IgnoreNameSpaces))
            {
                return IgnoringNameSpaces;
            }

            return Default;
        }

        public bool Equals(XName x, XName y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (this.options.HasFlag(XmlAssertOptions.IgnoreNameSpaces))
            {
                return x.LocalName == y.LocalName;
            }

            return x == y;
        }

        public int GetHashCode(XName obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            if (this.options.HasFlag(XmlAssertOptions.IgnoreNameSpaces))
            {
                return obj.LocalName.GetHashCode();
            }

            return obj.GetHashCode();
        }

        public int Compare(XName x, XName y)
        {
            if (x == y)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            if (this.options.HasFlag(XmlAssertOptions.IgnoreNameSpaces))
            {
                return string.CompareOrdinal(x.LocalName, y.LocalName);
            }

            var ns = string.CompareOrdinal(x.NamespaceName, y.NamespaceName);
            if (ns != 0)
            {
                return ns;
            }

            return string.CompareOrdinal(x.LocalName, y.LocalName);
        }
    }
}
