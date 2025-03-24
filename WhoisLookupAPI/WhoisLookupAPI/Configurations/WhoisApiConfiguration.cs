namespace WhoisLookupAPI.Configurations
{
    /// <summary>
    /// Represents the configuration settings for the Whois API.
    /// </summary>
    public class WhoisApiConfiguration
    {
        /// <summary>
        /// Gets or sets the base URL of the Whois API.
        /// </summary>
        public string BaseURL { get; set; }

        /// <summary>
        /// Gets or sets the API key used for authenticating requests to the Whois API.
        /// </summary>
        public string APIKey { get; set; }
    }
}
