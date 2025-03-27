namespace WhoisLookupAPI.ApiClients
{
    using Microsoft.AspNetCore.WebUtilities;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Provides methods to interact with the Whois API and retrieve domain information.
    /// Implements retry logic for handling transient failures.
    /// </summary>
    public class WhoisAPIClient : IWhoisAPIClient
    {
        private readonly HttpClient _httpClient;
        private readonly IBetterStackLogService _logger;
        private const int MaxRetries = 3; // Maximum retry attempts for transient failures

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisAPIClient"/> class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making API requests.</param>
        /// <param name="logger">An instance of <see cref="IBetterStackLogService"/> for logging API interactions and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
        public WhoisAPIClient(HttpClient httpClient, IBetterStackLogService logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown when <paramref name="domainName"/> is null or empty.</exception>
        /// <exception cref="WhoisAPIException">Thrown when the API request fails after multiple retries.</exception>
        public async Task<string> GetWhoisInfo(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                ArgumentException argEx = new ArgumentException("Domain name cannot be null or empty.", nameof(domainName));
                await _logger.LogErrorAsync(new { Exception = argEx, DomainName = domainName });
                throw argEx;
            }

            // Construct query parameters for the API request
            Dictionary<string, string?> queryParameters = new Dictionary<string, string?> {
                { "domainName", domainName },
                { "outputFormat", "JSON" }
            };

            // Note: Base URL is already set to the HTTP client via DI
            string endpoint = QueryHelpers.AddQueryString(string.Empty, queryParameters);

            int retryCount = 0;
            while (retryCount <= MaxRetries)
            {
                try
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }

                    // If the API returns a 429 Too Many Requests error, apply exponential backoff and retry
                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(ComputeExponentialBackoff(retryCount));
                    }
                    else
                    {
                        // Log and throw an exception for other HTTP errors
                        WhoisAPIException ex = new WhoisAPIException($"Whois API request failed with status code {response.StatusCode}", response.StatusCode);
                        await _logger.LogErrorAsync(new { Exception = ex, DomainName = domainName });
                        throw ex;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handles network-related issues (e.g., API is unreachable, connection failure)
                    WhoisAPIException apiEx = new WhoisAPIException("Failed to connect to Whois API.", HttpStatusCode.ServiceUnavailable, ex);
                    await _logger.LogErrorAsync(new { Exception = apiEx, DomainName = domainName });
                    throw apiEx;
                }
                catch (TaskCanceledException ex)
                {
                    // Handles timeout errors when the API takes too long to respond
                    WhoisAPIException apiEx = new WhoisAPIException("Request timeout. The API call took too long.", HttpStatusCode.RequestTimeout, ex);
                    await _logger.LogErrorAsync(new { Exception = apiEx, DomainName = domainName });
                    throw apiEx;
                }
                finally
                {
                    // Ensure the retry count is only incremented on failures
                    retryCount++;
                }
            }

            // Log and throw an exception after exhausting all retries
            WhoisAPIException apiException = new WhoisAPIException("Whois API request failed after multiple retries.", HttpStatusCode.ServiceUnavailable);
            await _logger.LogErrorAsync(new { Exception = apiException, DomainName = domainName });
            throw apiException;
        }

        /// <summary>
        /// Computes the delay time for exponential backoff retries based on the retry attempt count.
        /// </summary>
        /// <param name="retryAttempt">The current retry attempt number.</param>
        /// <returns>The time delay in milliseconds before the next retry attempt.</returns>
        private static int ComputeExponentialBackoff(int retryAttempt)
        {
            // Exponential backoff formula: 2^retryAttempt * 1000 (e.g., 1s, 2s, 4s, etc.)
            return (int)(Math.Pow(2, retryAttempt) * 1000);
        }
    }
}
