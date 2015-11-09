namespace Gu.SerializationAsserts
{
    using System.Collections.Generic;
    using System.Reflection;

    public class FieldComparison
    {
        private readonly List<FieldInfo> _path;
        private readonly List<object> _allChecked;

        public FieldComparison(object source)
        {
            Source = source;
            _path = new List<FieldInfo>();
            _allChecked = new List<object> { source };
        }

        public FieldComparison(List<FieldInfo> path, List<object> allChecked, object source)
        {
            _path = path;
            _allChecked = allChecked;
            _allChecked.Add(source);
            Source = source;
        }

        public object Source { get; }

        public IReadOnlyList<object> AllChecked => _allChecked;

        public IReadOnlyList<FieldInfo> Path => _path;
    }
}
