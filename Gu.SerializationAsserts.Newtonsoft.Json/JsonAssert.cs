namespace Gu.SerializationAsserts.Newtonsoft.Json
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using global::Newtonsoft.Json.Linq;

    /// <summary>For asserting that json stings are equal.</summary>
    public static class JsonAssert
    {
        /// <summary>
        /// Parses the Json and compares expected to actual.
        /// </summary>
        /// <param name="expected">The expected Json</param>
        /// <param name="actual">The actual Json</param>
        /// <param name="options">How to compare the Json</param>
        public static void Equal(string expected, string actual, JsonAssertOptions options = JsonAssertOptions.Default)
        {
            var expectedJson = ParseDocument(expected, nameof(expected), options);
            var actualJson = ParseDocument(actual, nameof(actual), options);
            Equal(expectedJson, actualJson, null, options);
        }

        /// <summary>
        /// Parses the Json and compares expected to actual.
        /// </summary>
        /// <param name="expected">The expected Json</param>
        /// <param name="actual">The actual Json</param>
        /// <param name="valueComparer">For custom value comparison.</param>
        /// <param name="options">How to compare the Json</param>
        public static void Equal(
            string expected,
            string actual,
            IEqualityComparer<JValue> valueComparer,
            JsonAssertOptions options = JsonAssertOptions.Verbatim)
        {
            var expectedJson = ParseDocument(expected, nameof(expected), options);
            var actualJson = ParseDocument(actual, nameof(actual), options);
            Equal(expectedJson, actualJson, valueComparer, options);
        }

        private static void Equal(JTokenAndSource expected, JTokenAndSource actual, IEqualityComparer<JValue> comparer, JsonAssertOptions options)
        {
            var expectedToken = expected?.JToken;
            var actualToken = actual?.JToken;
            if (expectedToken?.Type != actualToken?.Type)
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            CheckElementOrder(expected, actual, options);
            if (expected?.JProperty?.Name != actual?.JProperty?.Name)
            {
                var message = CreateMessage(expected, actual);
                throw new AssertException(message);
            }

            var expectedValue = expectedToken as JValue;
            var actualValue = actualToken as JValue;
            if (expectedValue != null || actualValue != null)
            {
                if (expectedValue == null || actualValue == null)
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }

                if (actualValue.HasValues)
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }

                var valueComparer = JValueComparer.GetFor(options);
                if (valueComparer.Equals(expectedValue, actualValue))
                {
                    return;
                }

                if (comparer?.Equals(expectedValue, actualValue) == true)
                {
                    return;
                }
                else
                {
                    var message = CreateMessage(expected, actual);
                    throw new AssertException(message);
                }
            }

            for (int i = 0; i < Math.Max(expected?.Children.Count ?? 0, actual?.Children.Count ?? 0); i++)
            {
                var expectedChild = expected?.Children.ElementAtOrDefault(i);
                var actualChild = actual?.Children.ElementAtOrDefault(i);
                Equal(expectedChild, actualChild, comparer, options);
            }
        }

        private static void CheckElementOrder(JTokenAndSource expected, JTokenAndSource actual, JsonAssertOptions options)
        {
            if (!options.HasFlag(JsonAssertOptions.Verbatim) || expected == null || actual == null)
            {
                return;
            }

            for (int i = 0; i < Math.Min(expected.Children.Count, actual.Children.Count); i++)
            {
                if (expected.Children[i].JProperty?.Name != actual.Children[i].JProperty?.Name &&
                    actual.Children.Any(x => x.JProperty?.Name == expected.Children[i].JProperty?.Name))
                {
                    var message = CreateMessage(expected.Children[i], actual.Children[i], "  The order of elements is incorrect.");
                    throw new AssertException(message);
                }
            }
        }

        private static JTokenAndSource ParseDocument(string json, string parameterName, JsonAssertOptions options)
        {
            try
            {
                var jObject = JToken.Parse(json);
                return new JTokenAndSource(json, jObject, options);
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException($"{parameterName} is not valid Json.", e);
            }
        }

        private static string CreateMessage(JTokenAndSource expected, JTokenAndSource actual, string subHeader = null)
        {
            var expectedLine = expected?.Json.Line(expected.LineNumber).Trim();
            var actualLine = actual?.Json.Line(actual.LineNumber).Trim();
            var index = expectedLine.FirstDiff(actualLine);
            var lineNumber = expected?.LineNumber ?? actual.LineNumber;
            using (var writer = new StringWriter())
            {
                if (subHeader != null)
                {
                    writer.Write(subHeader);
                    writer.WriteLine();
                }

                writer.WriteLine($"  Json differ at line {lineNumber} index {index}.");
                writer.WriteLine($"  Expected: {expected?.LineNumber}| {expectedLine}");
                writer.WriteLine($"  But was:  {actual?.LineNumber.ToString() ?? new string('?', expected?.LineNumber.ToString().Length ?? 0)}| {actualLine ?? "Missing"}");
                writer.Write($"  {new string('-', index + 13)}^");
                return writer.ToString();
            }
        }

        // Using new here to hide it so it not called by mistake
        // ReSharper disable once UnusedMember.Local
        private static new void Equals(object x, object y)
        {
            throw new AssertException($"{x}, {y}");
        }
    }
}
