namespace Gu.SerializationAsserts.Newtonsoft.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Newtonsoft.Json;
    using global::Newtonsoft.Json.Linq;

    internal class JTokenAndSource
    {
        public JTokenAndSource(string json, JToken jObject, JsonAssertOptions options)
        {
            this.Json = json;
            this.JToken = jObject;
            this.Options = options;
            var children = jObject.Children().Select(x => new JTokenAndSource(json, x, options));
            this.Children = options == JsonAssertOptions.Verbatim
                ? children.ToList()
                : children.OrderBy(x => x.PropertyName).ToList();
        }

        internal JToken JToken { get; }

        internal JProperty JProperty => this.JToken as JProperty;

        internal string PropertyName => this.JProperty?.Name;

        internal string Json { get; }

        internal JsonAssertOptions Options { get; }

        internal IReadOnlyList<JTokenAndSource> Children { get; }

        internal int LineNumber => (this.JToken as IJsonLineInfo)?.LineNumber ?? 0;

        internal int LinePosition => (this.JToken as IJsonLineInfo)?.LinePosition ?? 0;
    }
}