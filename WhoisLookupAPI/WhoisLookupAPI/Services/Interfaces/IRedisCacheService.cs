namespace WhoisLookupAPI.Services.Interfaces
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines methods for interacting with a Redis cache, enabling storage, retrieval, and removal of cached data.
    /// </summary>
    public interface IRedisCacheService
    {
        /// <summary>
        /// Asynchronously stores a value in Redis with an optional expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to be stored.</typeparam>
        /// <param name="key">A unique identifier for the cached value.</param>
        /// <param name="value">The data to cache.</param>
        /// <param name="expiry">The duration before the cached entry expires. If null, the entry persists indefinitely.</param>
        /// <returns>A task representing the asynchronous cache storage operation.</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Asynchronously retrieves a cached value from Redis.
        /// </summary>
        /// <typeparam name="T">The expected type of the cached value.</typeparam>
        /// <param name="key">A unique identifier for the cached entry.</param>
        /// <returns>A task that resolves to the cached value if found; otherwise, returns <c>default(T)</c>.</returns>
        Task<T> GetAsync<T>(string key);
    }
}
