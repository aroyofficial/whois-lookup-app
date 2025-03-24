namespace WhoisLookupAPI.Services
{
    using StackExchange.Redis;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Provides an implementation of <see cref="IRedisCacheService"/> for managing Redis-based caching operations.
    /// </summary>
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer instance.</param>
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        /// <inheritdoc/>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));

            if (value is null)
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");

            try
            {
                // Serialize object to JSON before storing in Redis
                var serializedValue = JsonSerializer.Serialize(value);
                await _database.StringSetAsync(key, serializedValue, expiry);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to cache data for key: {key}", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));

            try
            {
                var cachedValue = await _database.StringGetAsync(key);
                if (!cachedValue.HasValue)
                    return default!;

                // Deserialize JSON back to the expected type
                return JsonSerializer.Deserialize<T>(cachedValue!)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve cache data for key: {key}", ex);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));

            try
            {
                await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to remove cache entry for key: {key}", ex);
            }
        }
    }
}
