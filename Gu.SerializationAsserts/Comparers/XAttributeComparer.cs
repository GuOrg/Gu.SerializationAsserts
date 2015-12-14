namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class XAttributeComparer : IEqualityComparer<XAttribute>
    {
        private static readonly ConcurrentDictionary<XmlAssertOptions, XAttributeComparer> Cache = new ConcurrentDictionary<XmlAssertOptions, XAttributeComparer>(XmlAssertOptionsComparer.Default);
        private readonly XmlAssertOptions options;
        private readonly XNameComparer nameComparer;

        public XAttributeComparer(XmlAssertOptions options)
        {
            this.options = options;
            this.nameComparer = XNameComparer.GetFor(this.options);
        }

        public static XAttributeComparer GetFor(XmlAssertOptions options)
        {
            return Cache.GetOrAdd(options, x => new XAttributeComparer(x));
        }

        public bool Equals(XAttribute x, XAttribute y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null)
            {
                if (this.options.HasFlag(XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual))
                {
                    return string.IsNullOrEmpty(y.Value);
                }

                return false;
            }

            if (y == null)
            {
                if (this.options.HasFlag(XmlAssertOptions.TreatEmptyAndMissingElemensAsEqual))
                {
                    return string.IsNullOrEmpty(x.Value);
                }

                return false;
            }

            if (!this.nameComparer.Equals(x.Name, y.Name))
            {
                return false;
            }

            return x.Value == y.Value;
        }

        public int GetHashCode(XAttribute obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            unchecked
            {
                var hashCode = this.nameComparer.GetHashCode(obj.Name);
                hashCode = (hashCode * 397) ^ StringComparer.Ordinal.GetHashCode(obj.Value);
                return hashCode;
            }
        }
    }
}