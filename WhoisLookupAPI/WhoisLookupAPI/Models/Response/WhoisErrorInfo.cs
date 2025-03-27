namespace WhoisLookupAPI.Models.Response
{
    /// <summary>
    /// Represents the response when there is an error in the Whois API response.
    /// </summary>
    public class WhoisErrorInfo : WhoisRecord
    {
        /// <summary>
        /// Gets or sets the domain name that was queried.
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the message indicating that the domain does not exist.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status indicating that the domain is not found.
        /// </summary>
        public bool IsDomainNotFound { get; set; } = true;
    }
}
