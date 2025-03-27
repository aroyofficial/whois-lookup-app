namespace WhoisLookupAPI.Models.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the registrant information retrieved from the Whois API.
    /// </summary>
    public class WhoisRegistrant
    {
        /// <summary>
        /// Gets or sets the name of the registrant organization.
        /// </summary>
        [JsonProperty("organization")]
        public string Name { get; set; }
    }
}
