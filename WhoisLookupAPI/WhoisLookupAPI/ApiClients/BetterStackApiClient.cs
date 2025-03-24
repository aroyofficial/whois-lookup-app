namespace WhoisLookupAPI.ApiClients
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Exceptions;

    /// <summary>
    /// Client for interacting with the Better Stack Logtail API.
    /// </summary>
    public class BetterStackApiClient : IBetterStackApiClient
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterStackApiClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for sending logs.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> is null.</exception>
        public BetterStackApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Sends a log entry to Better Stack Logtail.
        /// </summary>
        /// <param name="level">The log level (info, warning, error).</param>
        /// <param name="message">The log message content.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        /// <exception cref="BetterStackLoggingException">Thrown when logging to Better Stack fails.</exception>
        public async Task SendLogAsync(string level, string message)
        {
            try
            {
                // Prepare log entry in JSON format
                var logEntry = new
                {
                    level,
                    message,
                    timestamp = DateTime.UtcNow.ToString("o") // ISO 8601 format
                };

                string json = JsonSerializer.Serialize(logEntry);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send log entry to Better Stack
                HttpResponseMessage response = await _httpClient.PostAsync(string.Empty, content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = $"Failed to send log to Better Stack. Status Code: {response.StatusCode}";
                    throw new BetterStackLoggingException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new BetterStackLoggingException("An error occurred while logging to Better Stack.", ex);
            }
        }
    }
}
