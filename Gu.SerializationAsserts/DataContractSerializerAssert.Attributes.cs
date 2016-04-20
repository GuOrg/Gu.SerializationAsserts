namespace Gu.SerializationAsserts
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>Exposes methods for assertions about attributes.</summary>
    public static partial class DataContractSerializerAssert
    {
        /// <summary>
        /// Checks that <typeparamref name="T"/> has <see cref="DataContract"/> defined.
        /// Throws if not.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public static void HasDataContractAttribute<T>()
        {
            HasDataContractAttribute(typeof(T));
        }

        /// <summary>
        /// Checks that <paramref name="type"/> has <see cref="DataContract"/> defined.
        /// Throws if not.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void HasDataContractAttribute(Type type)
        {
            if (!Attribute.IsDefined(type, typeof(DataContractAttribute)))
            {
                throw new AssertException($"  Expected type {type} to have [{nameof(DataContractAttribute)}]");
            }
        }

        /// <summary>
        /// Checks that all properties of <typeparamref name="T"/> has <see cref="DataMember"/> defined.
        /// Throws if not.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public static void AllPropertiesHasDataMemberAttributes<T>()
        {
            AllPropertiesHasDataMemberAttributes(typeof(T));
        }

        /// <summary>
        /// Checks that all properties of <paramref name="type"/> has <see cref="DataMember"/> defined.
        /// Throws if not.
        /// </summary>
        /// <param name="type">The type.</param>
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
