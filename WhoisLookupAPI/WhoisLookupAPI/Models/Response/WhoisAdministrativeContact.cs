namespace WhoisLookupAPI.Models.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the administrative contact information retrieved from the Whois API.
    /// </summary>
    public class WhoisAdministrativeContact
    {
        /// <summary>
        /// Gets or sets the name of the administrative contact organization.
        /// </summary>
        [JsonProperty("organization")]
        public string Name { get; set; }
    }
}
