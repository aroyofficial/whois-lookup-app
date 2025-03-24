namespace WhoisLookupAPI.Services
{
    using StackExchange.Redis;
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Implements a Redis-based caching service.
    /// </summary>
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;
        private readonly IBetterStackLogService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
        /// </summary>
        /// <param name="redis">The Redis connection multiplexer instance.</param>
        /// <param name="logger">The logging service instance.</param>
        public RedisCacheService(IConnectionMultiplexer redis, IBetterStackLogService logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null.");
            }

            try
            {
                await _logger.LogInfoAsync($"Storing key {key} into cache");
                string serializedValue = JsonSerializer.Serialize(value);
                await _database.StringSetAsync(key, serializedValue, expiry);
                await _logger.LogInfoAsync($"Key {key} stored successfully into cache");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key });
                throw new InvalidOperationException($"Failed to cache data for key: {key}", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));
            }

            try
            {
                await _logger.LogInfoAsync($"Finding key {key} in cache");
                RedisValue cachedValue = await _database.StringGetAsync(key);
                if (!cachedValue.HasValue)
                {
                    await _logger.LogInfoAsync($"Key {key} not found in cache");
                    return default!;
                }

                await _logger.LogInfoAsync($"Key {key} found in cache");
                return JsonSerializer.Deserialize<T>(cachedValue!)!;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key });
                throw new InvalidOperationException($"Failed to retrieve cache data for key: {key}", ex);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(key));
            }

            try
            {
                await _logger.LogInfoAsync($"Removing key {key} from cache");
                await _database.KeyDeleteAsync(key);
                await _logger.LogInfoAsync($"Key {key} removed successfully from cache");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key });
                throw new InvalidOperationException($"Failed to remove cache entry for key: {key}", ex);
            }
        }
    }
}
