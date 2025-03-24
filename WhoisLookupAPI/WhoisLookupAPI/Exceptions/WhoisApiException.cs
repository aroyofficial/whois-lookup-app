namespace WhoisLookupAPI.Exceptions
{
    using System;
    using System.Net;

    /// <summary>
    /// Exception for errors occurring while calling the Whois API.
    /// </summary>
    public class WhoisAPIException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the API error.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisAPIException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="innerException">Optional inner exception.</param>
        public WhoisAPIException(string message, HttpStatusCode statusCode, Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
