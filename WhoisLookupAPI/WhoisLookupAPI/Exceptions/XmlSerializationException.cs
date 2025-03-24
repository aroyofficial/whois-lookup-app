namespace WhoisLookupAPI.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when an error occurs during XML serialization or deserialization.
    /// </summary>
    public class XmlSerializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializationException"/> class.
        /// </summary>
        /// <param name="message">Error message describing the issue.</param>
        public XmlSerializationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlSerializationException"/> class with an inner exception.
        /// </summary>
        /// <param name="message">Error message describing the issue.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public XmlSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
