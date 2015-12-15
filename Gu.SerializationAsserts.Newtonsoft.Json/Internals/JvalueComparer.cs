namespace Gu.SerializationAsserts.Newtonsoft.Json
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using global::Newtonsoft.Json.Linq;

    internal class JValueComparer : IEqualityComparer<JValue>
    {
        private static readonly ConcurrentDictionary<JsonAssertOptions, JValueComparer> Cache = new ConcurrentDictionary<JsonAssertOptions, JValueComparer>();
        private readonly JsonAssertOptions options;

        public JValueComparer(JsonAssertOptions options)
        {
            this.options = options;
        }

        public static JValueComparer GetFor(JsonAssertOptions options)
        {
            return Cache.GetOrAdd(options, x => new JValueComparer(x));
        }

        public bool Equals(JValue x, JValue y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Type != JTokenType.Property || y.Type != JTokenType.Property)
            {
                return false;
            }

            return Equals(x.Value, y.Value);
        }

        public int GetHashCode(JValue obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            return obj.Value.GetHashCode();
        }
    }
}