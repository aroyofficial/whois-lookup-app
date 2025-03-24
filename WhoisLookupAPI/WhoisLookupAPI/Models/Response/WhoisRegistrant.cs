namespace WhoisLookupAPI.Models.Response
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the registrant information retrieved from the Whois API.
    /// </summary>
    public class WhoisRegistrant
    {
        /// <summary>
        /// Gets or sets the name of the registrant organization.
        /// </summary>
        [XmlElement("organization")]
        public string Name { get; set; }
    }
}
