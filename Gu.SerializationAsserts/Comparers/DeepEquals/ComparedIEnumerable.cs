namespace Gu.SerializationAsserts
{
    using System.Reflection;

    internal class ComparedIEnumerable : ICompared
    {
        public ComparedIEnumerable(object[] value, FieldInfo field)
        {
            this.Value = value;
            this.Field = field;
            this.Count = value.Length;
        }

        public object Value { get; }

        public FieldInfo Field { get; }

        public int Count { get; }
    }
}