namespace WhoisLookupAPI.Models.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the technical contact information retrieved from the Whois API.
    /// </summary>
    public class WhoisTechnicalContact
    {
        /// <summary>
        /// Gets or sets the name of the technical contact organization.
        /// </summary>
        [JsonProperty("organization")]
        public string Name { get; set; }
    }
}
