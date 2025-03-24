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
        /// An API error occurred while processing the request.
        /// </summary>
        ApiError = 2002,

        /// <summary>
        /// A network error occurred while communicating with an external service.
        /// </summary>
        NetworkError = 2003,

        /// <summary>
        /// The request timed out before completion.
        /// </summary>
        Timeout = 2004,

        /// <summary>
        /// An unknown error occurred.
        /// </summary>
        UnknownError = 2005,

        #endregion Server Errors
    }
}
