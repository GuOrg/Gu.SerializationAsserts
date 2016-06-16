namespace Gu.SerializationAsserts.Newtonsoft.Json.Tests.Dtos
{
    using System.Collections;

    public class Dummy
    {
        public static readonly IComparer Comparer = new DummyComparer();

        public Dummy()
        {
        }

        public Dummy(int value)
        {
            this.Value = value;
        }

        public int Value { get; set; }

        private class DummyComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                return ((Dummy)x).Value.CompareTo(((Dummy)y).Value);
            }
        }
    }
}