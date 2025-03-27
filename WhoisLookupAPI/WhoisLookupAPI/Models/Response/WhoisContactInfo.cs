namespace WhoisLookupAPI.Models.Response
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the contact information retrieved from the Whois API response.
    /// </summary>
    public class WhoisContactInfo : WhoisRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisContactInfo"/> class.
        /// This constructor ensures that the registrant, technical contact, 
        /// and administrative contact properties are properly initialized.
        /// </summary>
        public WhoisContactInfo()
        {
            _registrant = new WhoisRegistrant();
            _technicalContact = new WhoisTechnicalContact();
            _administrativeContact = new WhoisAdministrativeContact();
        }

        /// <summary>
        /// Gets or sets the registrant details, including the organization name.
        /// This property is private and mapped using JSON serialization.
        /// </summary>
        [JsonProperty("registrant")]
        private WhoisRegistrant _registrant { get; set; }

        /// <summary>
        /// Gets or sets the technical contact details, including the organization name.
        /// This property is private and mapped using JSON serialization.
        /// </summary>
        [JsonProperty("technicalContact")]
        private WhoisTechnicalContact _technicalContact { get; set; }

        /// <summary>
        /// Gets or sets the administrative contact details, including the organization name.
        /// This property is private and mapped using JSON serialization.
        /// </summary>
        [JsonProperty("administrativeContact")]
        private WhoisAdministrativeContact _administrativeContact { get; set; }

        /// <summary>
        /// Gets the registrant's name extracted from the private registrant details.
        /// </summary>
        public string RegistrantName => _registrant.Name;

        /// <summary>
        /// Gets the technical contact's name extracted from the private technical contact details.
        /// </summary>
        public string TechnicalContactName => _technicalContact.Name;

        /// <summary>
        /// Gets the administrative contact's name extracted from the private administrative contact details.
        /// </summary>
        public string AdministrativeContactName => _administrativeContact.Name;

        /// <summary>
        /// Gets or sets the contact email associated with the domain.
        /// </summary>
        [JsonProperty("contactEmail")]
        public string ContactEmail { get; set; }
    }
}
