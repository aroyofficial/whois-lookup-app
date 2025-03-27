namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when an error occurs while resolving environment variables in configuration.
    /// </summary>
    public class ConfigurationResolutionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolutionException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ConfigurationResolutionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationResolutionException"/> class with an inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception that caused this error.</param>
        public ConfigurationResolutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
