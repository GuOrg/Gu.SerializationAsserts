namespace Gu.SerializationAsserts
{
    using System;

    public static class FieldAssert
    {
        /// <summary>
        /// Checks:
        /// - All fields and nested fields.
        /// - All elements of IEnumerable fields.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void Equal<T>(T x, T y)
        {
            var comparison = Comparison.CreateFor(x, y);
            if (comparison.Matches())
            {
                return;
            }

            throw new AssertException("x and y not equal");
        }

        // Using new here to hide it so it not called by mistake
        private new static void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }
    }
}