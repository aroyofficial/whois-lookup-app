namespace WhoisLookupAPI.Models.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the name servers in the Whois response.
    /// </summary>
    public class WhoisNameServers
    {
        private string _hostNames;

        /// <summary>
        /// Gets or sets the list of hostnames.
        /// Truncated with "..." if longer than 25 characters.
        /// </summary>
        [JsonProperty("rawText")]
        public string Hostnames
        {
            get => _hostNames;
            set => _hostNames = value?.Length > 25 ? $"{value.Replace("\n", ",")[..23]}..." : value;
        }
    }
}
