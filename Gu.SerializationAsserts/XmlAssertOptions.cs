namespace Gu.SerializationAsserts
{
    using System;

    [Flags]
    public enum XmlAssertOptions
    {
        Verbatim = 0,
        IgnoreDeclaration = 1 << 0,
        IgnoreNamespaces = 1 << 1,
        IgnoreElementOrder = 1 << 2,
        IgnoreAttributeOrder = 1 << 3,
        IgnoreOrder = IgnoreElementOrder | IgnoreAttributeOrder,
        TreatEmptyAndMissingElemensAsEqual = 1 << 4,
        TreatEmptyAndMissingAttributesAsEqual = 1 << 4,
        TreatEmptyAndMissingAsEqual = TreatEmptyAndMissingElemensAsEqual | TreatEmptyAndMissingAttributesAsEqual,
    }
}