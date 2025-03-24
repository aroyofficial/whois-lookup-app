namespace WhoisLookupAPI.Enumerations
{
    /// <summary>
    /// Represents standard error codes used across the Whois API.
    /// </summary>
    public enum ErrorCode
    {
        #region Client Errors

        /// <summary>
        /// The request contains invalid or missing parameters.
        /// </summary>
        InvalidRequest = 1001,

        /// <summary>
        /// The domain name format is invalid.
        /// </summary>
        InvalidDomainFormat = 1002,

        /// <summary>
        /// The specified request type is not supported.
        /// </summary>
        UnsupportedRequestType = 1003,

        /// <summary>
        /// The requested domain information could not be found.
        /// </summary>
        DomainNotFound = 1004,

        #endregion Client Errors

        #region Server Errors

        /// <summary>
        /// An unexpected internal server error occurred.
        /// </summary>
        InternalServerError = 2001,

        /// <summary>
        /// An error occurred while interacting with the database.
        /// </summary>
        DatabaseError = 2002,

        /// <summary>
        /// The API configuration is missing or invalid.
        /// </summary>
        ConfigurationError = 2003,

        #endregion Server Errors

        #region External API Errors

        /// <summary>
        /// The external Whois API request failed.
        /// </summary>
        ExternalApiError = 3001,

        /// <summary>
        /// The provided API key is invalid.
        /// </summary>
        InvalidApiKey = 3002,

        /// <summary>
        /// The rate limit for API requests has been exceeded.
        /// </summary>
        RateLimitExceeded = 3003,

        /// <summary>
        /// The external Whois API returned an unexpected response.
        /// </summary>
        ExternalApiUnexpectedResponse = 3004,

        #endregion External API Errors

        #region Authentication & Authorization Errors

        /// <summary>
        /// The request is unauthorized due to missing or invalid authentication credentials.
        /// </summary>
        UnauthorizedAccess = 4001,

        /// <summary>
        /// Access to the requested resource is forbidden.
        /// </summary>
        Forbidden = 4002,

        #endregion Authentication & Authorization Errors

        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown = 5001,
    }
}
