namespace WhoisLookupAPI.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Provides utility methods for cache-related operations.
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// Generates a unique cache key by serializing an object and hashing the resulting string.
        /// </summary>
        /// <param name="obj">The object to generate the cache key for.</param>
        /// <returns>A hashed string representation of the object's serialized data.</returns>
        public static string GenerateKey(object obj)
        {
            string cacheKey = JsonConvert.SerializeObject(obj);
            SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cacheKey));
            string hashedKey = Convert.ToBase64String(hashBytes);
            return hashedKey;
        }
    }
}
