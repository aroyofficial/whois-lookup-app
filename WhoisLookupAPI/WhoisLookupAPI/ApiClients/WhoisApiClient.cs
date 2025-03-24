namespace WhoisLookupAPI.ApiClients
{
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Exceptions;

    /// <summary>
    /// Client for interacting with the Whois API to retrieve domain information.
    /// </summary>
    public class WhoisApiClient : IWhoisApiClient
    {
        private readonly HttpClient _httpClient;
        private const int MaxRetries = 3; // Maximum retry attempts for transient failures

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests.</param>
        /// <param name="configuration">API configuration options containing the base URL and API key.</param>
        /// <param name="logger">Logger for logging API interactions and errors.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="httpClient"/>, <paramref name="configuration"/>, or <paramref name="logger"/> is null.
        /// </exception>
        public WhoisApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Retrieves Whois information for the specified domain with retry logic.
        /// </summary>
        /// <param name="domainName">The domain name to fetch Whois information for.</param>
        /// <returns>A JSON string containing the Whois information.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="domainName"/> is null or empty.</exception>
        /// <exception cref="WhoisApiException">Thrown if the API request fails after retries.</exception>
        public async Task<string> GetWhoisInfo(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                throw new ArgumentException("Domain name cannot be null or empty.", nameof(domainName));
            }

            var queryParameters = new Dictionary<string, string?> { { "domainName", domainName } };
            string endpoint = QueryHelpers.AddQueryString(string.Empty, queryParameters);

            int retryCount = 0;
            while (retryCount < MaxRetries)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }

                    if (response.StatusCode == HttpStatusCode.TooManyRequests) // 429 Rate Limit Exceeded
                    {
                        await Task.Delay(ComputeExponentialBackoff(retryCount));
                    }
                    else
                    {
                        throw new WhoisApiException($"Whois API request failed with status code: {response.StatusCode}", response.StatusCode);
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new WhoisApiException("Failed to connect to Whois API.", HttpStatusCode.ServiceUnavailable, ex);
                }
                catch (TaskCanceledException ex)
                {
                    throw new WhoisApiException("Request timeout. The API call took too long.", HttpStatusCode.RequestTimeout, ex);
                }

                retryCount++;
            }

            throw new WhoisApiException("Whois API request failed after multiple retries.", HttpStatusCode.ServiceUnavailable);
        }

        /// <summary>
        /// Computes the delay time for exponential backoff retries.
        /// </summary>
        /// <param name="retryAttempt">The current retry attempt number.</param>
        /// <returns>A time delay in milliseconds before the next retry attempt.</returns>
        private static int ComputeExponentialBackoff(int retryAttempt)
        {
            return (int)(Math.Pow(2, retryAttempt) * 1000); // Exponential backoff: 1s, 2s, 4s
        }
    }
}
