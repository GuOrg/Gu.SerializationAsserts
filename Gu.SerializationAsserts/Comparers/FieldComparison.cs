namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Reflection;

    public class FieldComparison
    {
        private readonly List<FieldInfo> path;
        private readonly List<object> allChecked;

        public FieldComparison(object source)
        {
            this.Source = source;
            this.path = new List<FieldInfo>();
            this.allChecked = new List<object> { source };
        }

        public FieldComparison(List<FieldInfo> path, List<object> allChecked, object source)
        {
            this.path = path;
            this.allChecked = allChecked;
            this.allChecked.Add(source);
            this.Source = source;
        }

        public object Source { get; }

        public IReadOnlyList<object> AllChecked => this.allChecked;

        public IReadOnlyList<FieldInfo> Path => this.path;
    }
}
