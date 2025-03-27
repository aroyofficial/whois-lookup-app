namespace WhoisLookupAPI.Models.Response
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Represents the domain information retrieved from the Whois API.
    /// </summary>
    public class WhoisDomainInfo : WhoisRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisDomainInfo"/> class.
        /// </summary>
        public WhoisDomainInfo()
        {
            _nameServers = new WhoisNameServers();
        }

        /// <summary>
        /// Stores the name server details internally.
        /// </summary>
        [JsonProperty("nameServers")]
        private WhoisNameServers _nameServers;

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        [JsonProperty("domainName")]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the registrar name.
        /// </summary>
        [JsonProperty("registrarName")]
        public string? Registrar { get; set; }

        /// <summary>
        /// Gets or sets the registration date of the domain.
        /// </summary>
        [JsonProperty("createdDate")]
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date of the domain.
        /// </summary>
        [JsonProperty("expiresDate")]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the estimated age of the domain in days.
        /// </summary>
        [JsonProperty("estimatedDomainAge")]
        public int? EstimatedDomainAge { get; set; }

        /// <summary>
        /// Gets the host names associated with the domain.
        /// </summary>
        public string HostNames => _nameServers.Hostnames;
    }
}
