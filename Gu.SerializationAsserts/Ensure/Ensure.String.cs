#pragma warning disable SA1600 // Elements must be documented
// ReSharper disable UnusedParameter.Global
namespace Gu.SerializationAsserts
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>Ensures för string.</summary>
    internal static partial class Ensure
    {
        public static void IsMatch(string text, string pattern, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (!Regex.IsMatch(text, pattern))
            {
                throw new ArgumentException(parameterName);
            }
        }

        internal static void NotNullOrEmpty(string value, string parameterName)
        {
            Debug.Assert(!string.IsNullOrEmpty(parameterName), $"{nameof(parameterName)} cannot be null");
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
