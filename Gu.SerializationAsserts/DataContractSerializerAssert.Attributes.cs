namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    public static partial class DataContractSerializerAssert
    {
        public static void HasDataContractAttribute<T>()
        {
            HasDataContractAttribute(typeof(T));
        }

        public static void HasDataContractAttribute(Type type)
        {
            if (!Attribute.IsDefined(type, typeof(DataContractAttribute)))
            {
                throw new AssertException($"  Expected type {type} to have [{nameof(DataContractAttribute)}]");
            }
        }

        public static void AllPropertiesHasDataMemberAttributes<T>()
        {
            AllPropertiesHasDataMemberAttributes(typeof(T));
        }

        public static void AllPropertiesHasDataMemberAttributes(Type type)
        {
            var withMissingAttributes = type.GetProperties()
                                            .Where(x => !Attribute.IsDefined(x, typeof(DataMemberAttribute)))
                                            .ToList();
            if (withMissingAttributes.Any())
            {
                using (var writer = new StringWriter())
                {
                    writer.WriteLine($"  Expected all properties for type {type} to have [{nameof(DataMemberAttribute)}]");
                    writer.WriteLine("  The following properties does not:");
                    for (int i = 0; i < withMissingAttributes.Count; i++)
                    {
                        if (i != 0)
                        {
                            writer.WriteLine();
                        }

                        var propertyInfo = withMissingAttributes[i];
                        writer.Write($"    {propertyInfo.Name}");
                    }

                    throw new AssertException(writer.ToString());
                }
            }
        }
    }
}
