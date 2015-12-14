namespace Gu.SerializationAsserts
{
    public static class FieldComparer
    {
        public static FieldComparer<T> For<T>(T instance)
        {
            return FieldComparer<T>.Default;
        }
    }
}