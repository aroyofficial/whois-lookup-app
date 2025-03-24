namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when the Whois API response is invalid or cannot be processed.
    /// </summary>
    public class InvalidResponseDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResponseDataException"/> class.
        /// </summary>
        /// <param name="message">The error message describing the issue.</param>
        public InvalidResponseDataException(string message)
            : base(message) { }
    }
}
