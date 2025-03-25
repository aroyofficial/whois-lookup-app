namespace WhoisLookupAPI.Utilities
{
    using Newtonsoft.Json.Serialization;
    using Newtonsoft.Json;
    using System.Reflection;
    using System.Linq;

    /// <summary>
    /// Custom contract resolver to include only properties explicitly marked with [<see cref="JsonProperty"/>] 
    /// (whether public or private) during serialization and deserialization.
    /// </summary>
    public class WhoisJsonContractResolver : DefaultContractResolver
    {
        /// <inheritdoc/>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            // Check if the property has the [JsonProperty] attribute
            bool hasJsonPropertyAttribute = member.GetCustomAttributes(typeof(JsonPropertyAttribute), true).Any();

            if (!hasJsonPropertyAttribute)
            {
                return null; // Ignore properties without [JsonProperty] attribute
            }

            var property = base.CreateProperty(member, memberSerialization);

            // Allow private properties to be serialized/deserialized
            property.Readable = true;
            property.Writable = true;

            return property;
        }
    }
}