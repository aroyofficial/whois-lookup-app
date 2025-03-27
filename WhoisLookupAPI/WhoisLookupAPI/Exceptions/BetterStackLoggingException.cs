namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Represents an exception that occurs when logging to Better Stack Logtail fails.
    /// </summary>
    public class BetterStackLoggingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BetterStackLoggingException"/> class.
        /// </summary>
        /// <param name="message">The error message describing the failure.</param>
        public BetterStackLoggingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterStackLoggingException"/> class.
        /// </summary>
        /// <param name="message">The error message describing the failure.</param>
        /// <param name="innerException">The inner exception that caused this error.</param>
        public BetterStackLoggingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
