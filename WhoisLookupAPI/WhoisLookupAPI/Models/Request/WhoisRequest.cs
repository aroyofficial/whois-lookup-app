namespace WhoisLookupAPI.Models.Request
{
    using WhoisLookupAPI.Enumerations;

    /// <summary>
    /// Represents a request model for performing a Whois lookup.
    /// </summary>
    public class WhoisRequest
    {
        /// <summary>
        /// Gets or sets the domain name for which Whois information is requested.
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the type of Whois request (DomainInfo or ContactInfo).
        /// </summary>
        public WhoisRequestType Type { get; set; }
    }
}
