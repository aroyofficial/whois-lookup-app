namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when there is an error parsing the Whois API response.
    /// </summary>
    public class WhoisAPIResponseParsingException : Exception
    {
        /// <summary>
        /// Gets the raw Whois API response that caused the error.
        /// </summary>
        public string RawResponse { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisAPIResponseParsingException"/> class.
        /// </summary>
        /// <param name="message">The error message describing the issue.</param>
        /// <param name="rawResponse">The raw response from the Whois API.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        public WhoisAPIResponseParsingException(string message, string rawResponse, Exception innerException = null)
            : base(message, innerException)
        {
            RawResponse = rawResponse;
        }
    }
}
