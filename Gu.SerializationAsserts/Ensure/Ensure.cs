#pragma warning disable SA1600 // Elements must be documented
// ReSharper disable once UnusedParameter.Global
namespace Gu.SerializationAsserts
{
    using System;
    using System.Diagnostics;

    using JetBrains.Annotations;

    /// <summary>General ensures.</summary>
    internal static partial class Ensure
    {
        public static void IsTrue(bool condition, string parameterName, string message)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            Debug.Assert(!string.IsNullOrEmpty(message), $"{nameof(message)} cannot be null");
            if (!condition)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message, parameterName);
                }
                else
                {
                    throw new ArgumentException(parameterName);
                }
            }
        }

        internal static void NotNull<T>([NotNull]T value, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        internal static void IsFalse(bool condition, string parameterName, string message)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            Debug.Assert(!string.IsNullOrEmpty(message), $"{nameof(message)} cannot be null");
            if (condition)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message, parameterName);
                }
                else
                {
                    throw new ArgumentException(parameterName);
                }
            }
        }
    }
}
