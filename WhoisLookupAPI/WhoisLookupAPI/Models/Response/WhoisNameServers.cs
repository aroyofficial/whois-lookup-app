namespace WhoisLookupAPI.Models.Response
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the name servers in the Whois response.
    /// </summary>
    public class WhoisNameServers
    {
        [XmlArray("hostNames")]
        [XmlArrayItem("Address")]
        public List<string> Hostnames { get; set; }
    }
}
