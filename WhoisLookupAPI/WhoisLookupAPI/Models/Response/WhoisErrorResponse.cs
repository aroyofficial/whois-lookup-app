namespace WhoisLookupAPI.Models.Response
{
    using WhoisLookupAPI.Enumerations;

    /// <summary>
    /// Represents an error response for Whois API requests.
    /// </summary>
    public class WhoisErrorResponse
    {
        /// <summary>
        /// Gets or sets the application-defined error code.
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the user-friendly error message describing the issue.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the internal error message for debugging purposes.
        /// </summary>
        /// <remarks>
        /// This property contains the exception message but excludes the stack trace.  
        /// It should only be logged internally and not exposed to API consumers.
        /// </remarks>
        public string ErrorDetails { get; set; }
    }
}
