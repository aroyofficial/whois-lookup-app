namespace WhoisLookupAPI.Models.Response
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the technical contact information retrieved from the Whois API.
    /// </summary>
    public class WhoisTechnicalContact
    {
        /// <summary>
        /// Gets or sets the name of the technical contact organization.
        /// </summary>
        [XmlElement("organization")]
        public string Name { get; set; }
    }
}
