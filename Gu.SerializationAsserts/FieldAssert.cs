namespace Gu.SerializationAsserts
{
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
        public static bool Equals<T>(T x, T y)
        {
            return FieldsEqualsComparer<T>.Default.Equals(x, y);
        }
    }
}