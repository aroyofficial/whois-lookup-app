﻿namespace WhoisLookupAPI.ApiClients
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
    public class WhoisApiClient : IWhoisApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IBetterStackLogService _logger;
        private const int MaxRetries = 3; // Maximum retry attempts for transient failures

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making API requests.</param>
        /// <param name="logger">An instance of <see cref="IBetterStackLogService"/> for logging API interactions and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="httpClient"/> or <paramref name="logger"/> is null.</exception>
        public WhoisApiClient(HttpClient httpClient, IBetterStackLogService logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves Whois information for a specified domain with built-in retry logic.
        /// </summary>
        /// <param name="domainName">The domain name for which Whois information is requested.</param>
        /// <returns>A JSON string containing the Whois information.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="domainName"/> is null or empty.</exception>
        /// <exception cref="WhoisApiException">Thrown when the API request fails after multiple retries.</exception>
        public async Task<string> GetWhoisInfo(string domainName)
        {
            if (string.IsNullOrWhiteSpace(domainName))
            {
                ArgumentException argEx = new ArgumentException("Domain name cannot be null or empty.", nameof(domainName));
                await _logger.LogErrorAsync(new { Exception = argEx, DomainName = domainName });
                throw argEx;
            }

            await _logger.LogInfoAsync($"Fetching data from Whois API for {domainName}");
            var queryParameters = new Dictionary<string, string?> { { "domainName", domainName } };
            string endpoint = QueryHelpers.AddQueryString(string.Empty, queryParameters);

            int retryCount = 0;
            while (retryCount <= MaxRetries)
            {
                try
                {
                    if (retryCount > 0)
                    {
                        await _logger.LogInfoAsync($"Retry {retryCount} - {retryCount}");
                    }

                    HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        await _logger.LogInfoAsync($"Successfully fetched data from Whois API for {domainName}");
                        return await response.Content.ReadAsStringAsync();
                    }

                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(ComputeExponentialBackoff(retryCount));
                    }
                    else
                    {
                        WhoisApiException ex = new WhoisApiException($"Whois API request failed with status code {response.StatusCode}", response.StatusCode);
                        await _logger.LogErrorAsync(new { Exception = ex, DomainName = domainName });
                        throw ex;
                    }
                }
                catch (HttpRequestException ex)
                {
                    WhoisApiException apiEx = new WhoisApiException("Failed to connect to Whois API.", HttpStatusCode.ServiceUnavailable, ex);
                    await _logger.LogErrorAsync(new { Exception = apiEx, DomainName = domainName });
                    throw apiEx;
                }
                catch (TaskCanceledException ex)
                {
                    WhoisApiException apiEx = new WhoisApiException("Request timeout. The API call took too long.", HttpStatusCode.RequestTimeout, ex);
                    await _logger.LogErrorAsync(new { Exception = apiEx, DomainName = domainName });
                    throw apiEx;
                }
                finally
                {
                    retryCount++;
                }
            }

            WhoisApiException apiException = new WhoisApiException("Whois API request failed after multiple retries.", HttpStatusCode.ServiceUnavailable);
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
            return (int)(Math.Pow(2, retryAttempt) * 1000); // Exponential backoff: 1s, 2s, 4s, etc.
        }
    }
}