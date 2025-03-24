namespace WhoisLookupAPI.Configurations
{
    /// <summary>
    /// Represents the configuration settings for Redis.
    /// </summary>
    public class RedisConfiguration
    {
        /// <summary>
        /// Gets or sets the Redis server endpoint.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the password for Redis authentication.
        /// </summary>
        public string Password { get; set; }
    }
}
