namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    /// <inheritdoc/>
    public class XNameComparer : IEqualityComparer<XName>, IComparer<XName>
    {
        /// <summary>An <see cref="XNameComparer"/> that ignores namespaces.</summary>
        public static readonly XNameComparer IgnoringNamespaces = new XNameComparer(XmlAssertOptions.IgnoreNamespaces);

        /// <summary>The default instance. Uses <see cref="XmlAssertOptions.Verbatim"/>.</summary>
        public static readonly XNameComparer Default = new XNameComparer(XmlAssertOptions.Verbatim);

        private readonly XmlAssertOptions options;

        private XNameComparer(XmlAssertOptions options)
        {
            this.options = options;
        }

        /// <summary>Returns a cached <see cref="XNameComparer"/></summary>
        public static XNameComparer GetFor(XmlAssertOptions options)
        {
            if (options.HasFlag(XmlAssertOptions.IgnoreNamespaces))
            {
                return IgnoringNamespaces;
            }

            return Default;
        }

        /// <inheritdoc/>
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

            if (this.options.HasFlag(XmlAssertOptions.IgnoreNamespaces))
            {
                return x.LocalName == y.LocalName;
            }

            return x == y;
        }

        /// <inheritdoc/>
        public int GetHashCode(XName obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            if (this.options.HasFlag(XmlAssertOptions.IgnoreNamespaces))
            {
                return obj.LocalName.GetHashCode();
            }

            return obj.GetHashCode();
        }

        /// <inheritdoc/>
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

            if (this.options.HasFlag(XmlAssertOptions.IgnoreNamespaces))
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
