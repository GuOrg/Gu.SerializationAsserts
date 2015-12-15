namespace Gu.SerializationAsserts
{
    public static class BinaryComparer
    {
        public static BinaryComparer<T> For<T>(T instance)
        {
            return BinaryComparer<T>.Default;
        }
    }
}