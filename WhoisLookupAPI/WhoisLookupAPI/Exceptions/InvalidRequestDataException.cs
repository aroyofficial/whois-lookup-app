namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when the request data is invalid.
    /// </summary>
    public class InvalidRequestDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestDataException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InvalidRequestDataException(string message)
            : base(message)
        {
        }
    }
}
