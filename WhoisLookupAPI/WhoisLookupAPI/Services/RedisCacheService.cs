namespace WhoisLookupAPI.Services
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using StackExchange.Redis;
    using System;
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
                Exception ex = new ArgumentException("Cache key cannot be null or empty.", nameof(key));
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key, Value = value });
                return;
            }

            if (value is null)
            {
                Exception ex = new ArgumentNullException(nameof(value), "Value cannot be null.");
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key, Value = value });
                return;
            }

            try
            {
                string serializedValue = JsonConvert.SerializeObject(value);
                await _database.StringSetAsync(key, serializedValue, expiry);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key, Value = value });
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Exception ex = new ArgumentException("Cache key cannot be null or empty.", nameof(key));
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key });
                return default;
            }

            try
            {
                RedisValue cachedValue = await _database.StringGetAsync(key);
                if (!cachedValue.HasValue)
                {
                    return default!;
                }

                return JsonConvert.DeserializeObject<T>(cachedValue!)!;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(new { Exception = ex, CacheKey = key });
                return default;
            }
        }
    }
}
