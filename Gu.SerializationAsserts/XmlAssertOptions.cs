namespace Gu.SerializationAsserts
{
    using System;

    [Flags]
    public enum XmlAssertOptions
    {
        Verbatim = 0,
        IgnoreDeclaration = 1 << 0,
        IgnoreNameSpaces = 1 << 1,
        IgnoreElementOrder = 1 << 2,
        IgnoreAttributeOrder = 1 << 3,
        IgnoreOrder = IgnoreElementOrder | IgnoreAttributeOrder
    }
}