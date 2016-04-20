namespace Gu.SerializationAsserts
{
    public static class XmlComparer
    {
        /// <summary>returns XmlComparer&lt;T&gt;.Default.</summary>
        // ReSharper disable once UnusedParameter.Global
        public static XmlComparer<T> For<T>(T instance)
        {
            return XmlComparer<T>.Default;
        }
    }
}