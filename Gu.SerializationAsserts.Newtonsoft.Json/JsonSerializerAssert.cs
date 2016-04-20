namespace Gu.SerializationAsserts.Newtonsoft.Json
{
    using System;
    using global::Newtonsoft.Json;

    /// <summary> Test serialization using  <see cref="JsonSerializer"/> </summary>
    public static class JsonSerializerAssert
    {
        /// <summary>
        /// 1. serializes <paramref name="expected"/> and <paramref name="actual"/> to Json strings using <see cref="JsonSerializer"/>
        /// 2. Compares the Json using <see cref="JsonAssert"/>
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        public static void Equal<T>(T expected, T actual)
        {
            var eJson = ToJson(expected, nameof(expected), null);
            var aJson = ToJson(actual, nameof(actual), null);
            JsonAssert.Equal(eJson, aJson);
        }

        /// <summary>
        /// 1. serializes <paramref name="expected"/> and <paramref name="actual"/> to Json strings using <see cref="JsonSerializer"/>
        /// 2. Compares the Json using <see cref="JsonAssert"/>
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="settings">The settings to use when serializing and deserializing</param>
        public static void Equal<T>(T expected, T actual, JsonSerializerSettings settings)
        {
            var eJson = ToJson(expected, nameof(expected), settings);
            var aJson = ToJson(actual, nameof(actual), settings);
            JsonAssert.Equal(eJson, aJson);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an Json string using <see cref="JsonSerializer"/>
        /// 2 Compares the Json with <paramref name="expectedJson"/>
        /// 3 Creates a <see cref="ContainerClass{T}"/>
        /// 4 Serializes it to Json.
        /// 5 Compares the Json
        /// 6 Deserializes it to container class
        /// 7 Does 2 &amp; 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedJson">The expected Json</param>
        /// <param name="actual">The actual item</param>
        /// <param name="options">How to compare the Json</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(string expectedJson, T actual, JsonAssertOptions options = JsonAssertOptions.Verbatim)
        {
            var actualJson = ToJson(actual, nameof(actual), null);
            JsonAssert.Equal(expectedJson, actualJson, options);
            return Roundtrip(actual);
        }

        /// <summary>
        /// 1 Serializes <paramref name="actual"/> to an Json string using <see cref="JsonSerializer"/>
        /// 2 Compares the Json with <paramref name="expectedJson"/>
        /// 3 Creates a <see cref="ContainerClass{T}"/>
        /// 4 Serializes it to Json.
        /// 5 Compares the Json
        /// 6 Deserializes it to container class
        /// 7 Does 2 &amp; 3 again, we repeat this to catch any errors from deserializing
        /// 8 Returns roundtripped instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="expectedJson">The expected Json</param>
        /// <param name="actual">The actual item</param>
        /// <param name="settings">The settings to use when deserializing</param>
        /// <param name="options">How to compare the Json</param>
        /// <returns>The roundtripped instance</returns>
        public static T Equal<T>(string expectedJson, T actual, JsonSerializerSettings settings, JsonAssertOptions options = JsonAssertOptions.Verbatim)
        {
            var actualJson = ToJson(actual, nameof(actual), settings);
            JsonAssert.Equal(expectedJson, actualJson, options);
            return Roundtrip(actual, settings);
        }

        /// <summary>
        /// 1. Places <paramref name="item"/> in a ContainerClass{T} container1
        /// 2. Serializes container1
        /// 3. Deserializes the containerJson to container2 and does FieldAssert.Equal(container1, container2);
        /// 4. Serializes container2
        /// 5. Checks JsonAssert.Equal(containerJson1, containerJson2, JsonAssertOptions.Verbatim);
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item"/></typeparam>
        /// <param name="item">The instance to roundtrip</param>
        /// <returns>The serialized and deserialized instance (container2.Other)</returns>
        public static T Roundtrip<T>(T item)
        {
            Roundtripper.Simple(item, nameof(item), ToJson, FromJson<T>);
            var roundtripped = Roundtripper.InContainer(
                item,
                nameof(item),
                ToJson,
                FromJson<ContainerClass<T>>,
                (e, a) => JsonAssert.Equal(e, a, JsonAssertOptions.Verbatim));

            FieldAssert.Equal(item, roundtripped);

            return roundtripped;
        }

        /// <summary>
        /// 1. Places <paramref name="item"/> in a ContainerClass{T} container1
        /// 2. Serializes container1
        /// 3. Deserializes the containerJson to container2 and does FieldAssert.Equal(container1, container2);
        /// 4. Serializes container2
        /// 5. Checks JsonAssert.Equal(containerJson1, containerJson2, JsonAssertOptions.Verbatim);
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item"/></typeparam>
        /// <param name="item">The instance to roundtrip</param>
        /// <param name="settings">The settings to use when serializing and deserializing.</param>
        /// <returns>The serialized and deserialized instance (container2.Other)</returns>
        public static T Roundtrip<T>(T item, JsonSerializerSettings settings)
        {
            Roundtripper.Simple(item, nameof(item), x => ToJson(x, settings), x => FromJson<T>(x, settings));
            var roundtripped = Roundtripper.InContainer(
                item,
                nameof(item),
                x => ToJson(x, settings),
                x => FromJson<ContainerClass<T>>(x, settings),
                (e, a) => JsonAssert.Equal(e, a, JsonAssertOptions.Verbatim));

            FieldAssert.Equal(item, roundtripped);

            return roundtripped;
        }

        /// <summary>
        /// 1. Creates an JsonSerializer(typeof(T))
        /// 2. Serialize <paramref name="item"/>
        /// 3. Returns the Json
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="item">The item to serialize</param>
        /// <returns>The Json representation of <paramref name="item>"/></returns>
        public static string ToJson<T>(T item)
        {
            return ToJson(item, nameof(item), null);
        }

        /// <summary>
        /// 1. Creates an JsonSerializer(typeof(T))
        /// 2. Serialize <paramref name="item"/>
        /// 3. Returns the Json
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="item">The item to serialize</param>
        /// <param name="settings">The settings to use when serializing</param>
        /// <returns>The Json representation of <paramref name="item>"/></returns>
        public static string ToJson<T>(T item, JsonSerializerSettings settings)
        {
            return ToJson(item, nameof(item), settings);
        }

        /// <summary>
        /// Get copy paste friendly Json for <paramref name="item"/>
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item"/></typeparam>
        /// <param name="item">The item to serialize</param>
        /// <returns>Json escaped and ready to paste in code.</returns>
        public static string ToEscapedJson<T>(T item)
        {
            var json = ToJson(item);
            return json.Escape(); // wasteful allocation here but np I think;
        }

        /// <summary>
        /// Get copy paste friendly Json for <paramref name="item"/>
        /// </summary>
        /// <typeparam name="T">The type of <paramref name="item"/></typeparam>
        /// <param name="item">The item to serialize</param>
        /// <param name="settings">The settings to use when serializing</param>
        /// <returns>Json escaped and ready to paste in code.</returns>
        public static string ToEscapedJson<T>(T item, JsonSerializerSettings settings)
        {
            var json = ToJson(item, settings);
            return json.Escape(); // wasteful allocation here but np I think;
        }

        /// <summary>
        /// 1. Creates an JsonSerializer(typeof(T))
        /// 2. Deserialize <paramref name="json"/>
        /// 3. Returns the deserialized instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="json">The string containing the Json</param>
        /// <returns>The deserialized instance</returns>
        public static T FromJson<T>(string json)
        {
            return FromJson<T>(json, nameof(json), null);
        }

        /// <summary>
        /// 1. Creates an JsonSerializer(typeof(T))
        /// 2. Deserialize <paramref name="json"/>
        /// 3. Returns the deserialized instance
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="json">The string containing the Json</param>
        /// <param name="settings">The settings to use when deserializing</param>
        /// <returns>The deserialized instance</returns>
        public static T FromJson<T>(string json, JsonSerializerSettings settings)
        {
            return FromJson<T>(json, nameof(json), settings);
        }

        private static T FromJson<T>(string json, string parameterName, JsonSerializerSettings settings)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException($"Could not deserialize {parameterName} to an instance of type {typeof(T)}", e);
            }
        }

        private static string ToJson<T>(T item, string parameterName, JsonSerializerSettings settings)
        {
            try
            {
                return JsonConvert.SerializeObject(item, settings);
            }
            catch (Exception e)
            {
                throw AssertException.CreateFromException($"Could not serialize{parameterName}.", e);
            }
        }

        // Using new here to hide it so it not called by mistake
        private static new void Equals(object x, object y)
        {
            throw new NotSupportedException($"{x}, {y}");
        }
    }
}