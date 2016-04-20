#pragma warning disable SA1600 // Elements must be documented
namespace Gu.SerializationAsserts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>Ensures for collections</summary>
    internal static partial class Ensure
    {
        internal static void NotNullOrEmpty<T>(IReadOnlyCollection<T> value, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            NotNull(value, parameterName);

            if (value.Count == 0)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        internal static void MinCount<T>(IReadOnlyCollection<T> value, int min, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            NotNull(value, parameterName);

            if (value.Count < min)
            {
                var message = $"Expected {nameof(value)}.{nameof(value.Count)} to be at least {min}";
                throw new ArgumentException(parameterName, message);
            }
        }

        internal static void MaxCount<T>(IReadOnlyCollection<T> value, int max, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            NotNull(value, parameterName);

            if (value.Count > max)
            {
                var message = $"Expected {nameof(value)}.{nameof(value.Count)} to be less than {max}";
                throw new ArgumentException(parameterName, message);
            }
        }
    }
}
