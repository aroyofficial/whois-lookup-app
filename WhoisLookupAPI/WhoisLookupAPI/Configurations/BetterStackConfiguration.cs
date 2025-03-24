namespace WhoisLookupAPI.Configurations
{
    /// <summary>
    /// Represents the configuration settings for Better Stack logging integration.
    /// </summary>
    public class BetterStackConfiguration
    {
        /// <summary>
        /// Gets or sets the Better Stack log ingestion URL.
        /// This URL is used to send logs to Better Stack's logging service.
        /// </summary>
        public string IngestionURL { get; set; }

        /// <summary>
        /// Gets or sets the API token used for authentication with Better Stack.
        /// This token is required to securely send logs to Better Stack.
        /// </summary>
        public string APIToken { get; set; }
    }
}
