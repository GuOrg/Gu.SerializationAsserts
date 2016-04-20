namespace Gu.SerializationAsserts
{
    using System;
    using System.Linq;

    /// <summary>Extension methods for <see cref="Type"/>.</summary>
    internal static class TypeExt
    {
        /// <summary>
        /// return type.Implements(typeof(IEquatable&lt;&gt;), type);
        /// </summary>
        /// <param name="type">The Type.</param>
        /// <returns>True if <paramref name="type"/> implements IEquatable&lt;&gt;</returns>
        internal static bool IsEquatable(this Type type)
        {
            return type.Implements(typeof(IEquatable<>), type);
        }

        /// <summary>
        /// To check if type implements IEquatable{string}
        /// Call like this type.Implements(typeof(IEquatable{}, typeof(string))
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="genericInterface">The open interface type ex : IEnumerable&lt;&gt;</param>
        /// <param name="genericArgument">The generic argument for <paramref name="genericInterface"/></param>
        /// <returns>True if <paramref name="type"/> implements the interface.</returns>
        internal static bool Implements(this Type type, Type genericInterface, Type genericArgument)
        {
            if (type.IsInterface &&
                type.IsGenericType(genericInterface, genericArgument))
            {
                return true;
            }

            var interfaces = type.GetInterfaces();
            return interfaces.Any(i => i.IsGenericType(genericInterface, genericArgument));
        }

        private static bool IsGenericType(this Type type, Type genericTypeDefinition, Type genericArgument)
        {
            Ensure.IsTrue(genericTypeDefinition.IsGenericType, nameof(genericTypeDefinition), $"{nameof(genericTypeDefinition)}.{nameof(genericTypeDefinition.IsGenericType)} must be true");

            if (!type.IsGenericType)
            {
                return false;
            }

            var gtd = type.GetGenericTypeDefinition();
            if (gtd != genericTypeDefinition)
            {
                return false;
            }

            var genericArguments = type.GetGenericArguments();
            return genericArguments.Length == 1 && genericArguments[0] == genericArgument;
        }
    }
}
