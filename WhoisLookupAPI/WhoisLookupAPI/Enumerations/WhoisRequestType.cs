namespace WhoisLookupAPI.Enumerations
{
    /// <summary>
    /// Specifies the type of Whois request.
    /// </summary>
    public enum WhoisRequestType
    {
        /// <summary>
        /// Retrieves domain-related information such as registrar, registration date, expiration date, and hostnames.
        /// </summary>
        DomainInfo,

        /// <summary>
        /// Retrieves contact-related information such as registrant name, technical contact, administrative contact, and email.
        /// </summary>
        ContactInfo
    }
}
