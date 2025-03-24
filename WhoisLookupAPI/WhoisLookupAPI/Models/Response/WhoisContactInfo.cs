namespace WhoisLookupAPI.Models.Response
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the contact information retrieved from the Whois API response.
    /// </summary>
    public class WhoisContactInfo : WhoisRecord
    {
        /// <summary>
        /// Gets or sets the registrant details, including the organization name.
        /// </summary>
        [XmlElement("registrant")]
        public WhoisRegistrant Registrant { get; set; }

        /// <summary>
        /// Gets or sets the technical contact details, including the organization name.
        /// </summary>
        [XmlElement("technicalContact")]
        public WhoisTechnicalContact TechnicalContact { get; set; }

        /// <summary>
        /// Gets or sets the administrative contact details, including the organization name.
        /// </summary>
        [XmlElement("administrativeContact")]
        public WhoisAdministrativeContact AdministrativeContact { get; set; }

        /// <summary>
        /// Gets or sets the contact email associated with the domain.
        /// </summary>
        [XmlElement("contactEmail")]
        public string ContactEmail { get; set; }
    }
}
