namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when JSON deserialization fails due to an invalid format.
    /// </summary>
    public class InvalidJsonFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidJsonFormatException"/> class 
        /// with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message describing the issue.</param>
        /// <param name="innerException">The exception that caused this error.</param>
        public InvalidJsonFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
