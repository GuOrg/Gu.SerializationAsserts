namespace Gu.SerializationAsserts
{
    using System;

    /// <summary>Specifies how comparison is performed.</summary>
    [Flags]
    public enum XmlAssertOptions
    {
        /// <summary>Requires an exact match of the xml.</summary>
        Verbatim = 0,

        /// <summary><see cref="System.Xml.XmlDeclaration"/> can be missing.</summary>
        IgnoreDeclaration = 1 << 0,

        /// <summary>Ignores xml namespeces.</summary>
        IgnoreNamespaces = 1 << 1,

        /// <summary>Ignrores order of xml elements.</summary>
        IgnoreElementOrder = 1 << 2,

        /// <summary>Ignores order of xml attributes.</summary>
        IgnoreAttributeOrder = 1 << 3,

        /// <summary>Ignores order for xml elements and attributes.</summary>
        IgnoreOrder = IgnoreElementOrder | IgnoreAttributeOrder,

        /// <summary>Treats an empty xml element and a missing element as null and hence equal.</summary>
        TreatEmptyAndMissingElemensAsEqual = 1 << 4,

        /// <summary>Treats an empty xml attribute and a missing attribute as null and hence equal.</summary>
        TreatEmptyAndMissingAttributesAsEqual = 1 << 5,

        /// <summary>Treats an empty xml elements and attributes and a missing elements and attributes as null and hence equal.</summary>
        TreatEmptyAndMissingAsEqual = TreatEmptyAndMissingElemensAsEqual | TreatEmptyAndMissingAttributesAsEqual
    }
}