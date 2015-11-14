namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class XElementComparer : IEqualityComparer<XElement>
    {
        private static readonly ConcurrentDictionary<XmlAssertOptions, XElementComparer> Cache = new ConcurrentDictionary<XmlAssertOptions, XElementComparer>(XmlAssertOptionsComparer.Default);
        private readonly XmlAssertOptions options;
        private readonly XNameComparer nameComparer;

        public XElementComparer(XmlAssertOptions options)
        {
            this.options = options;
            this.nameComparer = XNameComparer.GetFor(this.options);
        }

        public static XElementComparer GetFor(XmlAssertOptions options)
        {
            return Cache.GetOrAdd(options, x => new XElementComparer(x));
        }

        public bool Equals(XElement x, XElement y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null)
            {
                if (this.options.HasFlag(XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual))
                {
                    return IsEmpty(y);
                }

                return false;
            }

            if (y == null)
            {
                if (this.options.HasFlag(XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual))
                {
                    return IsEmpty(x);
                }

                return false;
            }

            if (!this.nameComparer.Equals(x.Name, y.Name))
            {
                return false;
            }

            return x.Value == y.Value;
        }

        public int GetHashCode(XElement obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            unchecked
            {
                var hashCode = this.nameComparer.GetHashCode(obj.Name);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(obj.Value);
                return hashCode;
            }
        }

        private static bool IsEmpty(XElement x)
        {
            if (x.HasAttributes)
            {
                return false;
            }

            if (x.HasElements)
            {
                return false;
            }

            return string.IsNullOrEmpty(x.Value);
        }
    }
}
