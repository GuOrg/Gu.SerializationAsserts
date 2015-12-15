namespace Gu.SerializationAsserts
{
    public static class XmlComparer
    {
        public static XmlComparer<T> For<T>(T instance)
        {
            return XmlComparer<T>.Default;
        }
    }
}