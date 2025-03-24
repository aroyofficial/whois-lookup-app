namespace WhoisLookupAPI.Models.Response
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the administrative contact information retrieved from the Whois API.
    /// </summary>
    public class WhoisAdministrativeContact
    {
        /// <summary>
        /// Gets or sets the name of the administrative contact organization.
        /// </summary>
        [XmlElement("organization")]
        public string Name { get; set; }
    }
}
