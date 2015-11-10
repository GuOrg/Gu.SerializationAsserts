namespace Gu.SerializationAsserts
{
    using System.Reflection;

    internal class ComparedField : ICompared
    {
        public ComparedField(object value, FieldInfo parentField)
        {
            this.Value = value;
            this.ParentField = parentField;
        }

        public object Value { get; }

        public FieldInfo ParentField { get;  }
    }
}