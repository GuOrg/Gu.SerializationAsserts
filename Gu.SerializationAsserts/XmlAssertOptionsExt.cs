namespace Gu.SerializationAsserts
{
    internal static class XmlAssertOptionsExt
    {
        internal static bool IsSet(this XmlAssertOptions options, XmlAssertOptions flag)
        {
            return (options & flag) != 0;
        }

        internal static bool IsAnySet(this XmlAssertOptions options, XmlAssertOptions flag1, XmlAssertOptions flag2)
        {
            return IsSet(options, flag1) || IsSet(options, flag2);
        }
    }
}