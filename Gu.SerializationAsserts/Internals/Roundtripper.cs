using System.Xml.Serialization;

namespace Gu.SerializationAsserts
{
    using System;

    internal static class Roundtripper
    {
        internal static TValue Simple<TValue, TData>(
            TValue item,
            string paremeterName,
            Func<TValue, TData> toData,
            Func<TData, TValue> fromData)
        {
            var data = toData(item);
            var roundtripped = fromData(data);
            try
            {
                FieldAssert.Equal(item, roundtripped);
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException(
                    $"Simple roundtrip failed. {paremeterName} is not equal to {roundtripped}",
                    e);
            }

            return roundtripped;
        }

        internal static TValue InContainer<TValue, TData>(
            TValue item,
            string paremeterName,
            Func<ContainerClass<TValue>, TData> toData,
            Func<TData, ContainerClass<TValue>> fromData,
            Action<TData, TData> isDataEqual)
        {
            try
            {
                var container1 = new ContainerClass<TValue>(item);
                var containerData1 = toData(container1);

                // doing it twice to catch errors when deserializing
                var container2 = fromData(containerData1);
                FieldAssert.Equal(container1, container2);

                var containerXml2 = toData(container2);
                isDataEqual(containerData1, containerXml2);

                return container2.Other;
            }
            catch (Exception e)
            {
                var message = $"  Roundtrip of {paremeterName} in {nameof(ContainerClass<TValue>)} Failed.\r\n" +
                               "  This means there is an error in serialization.\r\n" +
                              $"  If you are implementing {nameof(IXmlSerializable)} check that you handle ReadEndElement properly as it is a common source of bugs.";
                throw new AssertException(message, e);
            }
        }
    }
}
