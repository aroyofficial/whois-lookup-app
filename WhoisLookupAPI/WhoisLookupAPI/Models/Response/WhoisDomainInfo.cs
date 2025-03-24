namespace WhoisLookupAPI.Models.Response
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the domain information retrieved from the Whois API.
    /// </summary>
    public class WhoisDomainInfo : WhoisRecord
    {
        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        [XmlElement("domainName")]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the registrar name.
        /// </summary>
        [XmlElement("registrarName")]
        public string RegistrarName { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        [XmlElement("createdDate")]
        public DateTimeOffset? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        [XmlElement("expiresDate")]
        public DateTimeOffset? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the estimated domain age in years.
        /// </summary>
        [XmlElement("estimatedDomainAge")]
        public int EstimatedDomainAge { get; set; }

        /// <summary>
        /// Gets or sets the list of hostnames.
        /// Truncated with "..." if longer than 25 characters.
        /// </summary>
        [XmlElement("nameServers")]
        public WhoisNameServers Hostnames { get; set; }
    }
}
