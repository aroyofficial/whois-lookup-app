namespace WhoisLookupAPI.Services
{
    using System;
    using System.Threading.Tasks;
    using WhoisLookupAPI.ApiClients;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Constants;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Service for logging messages to Better Stack Logtail.
    /// </summary>
    public class BetterStackLogService : IBetterStackLogService
    {
        private readonly IBetterStackApiClient _apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterStackLogService"/> class.
        /// </summary>
        /// <param name="apiClient">The API client responsible for sending logs to Better Stack Logtail.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiClient"/> is null.</exception>
        public BetterStackLogService(IBetterStackApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        /// <summary>
        /// Logs an informational message to Better Stack Logtail.
        /// </summary>
        /// <param name="message">The log message to be recorded.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        public async Task LogInfoAsync(string message)
        {
            await _apiClient.SendLogAsync(BetterStackLogLevel.Info, message);
        }

        /// <summary>
        /// Logs a warning message to Better Stack Logtail.
        /// </summary>
        /// <param name="message">The log message indicating a warning event.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        public async Task LogWarningAsync(string message)
        {
            await _apiClient.SendLogAsync(BetterStackLogLevel.Warning, message);
        }

        /// <summary>
        /// Logs an error message to Better Stack Logtail.
        /// </summary>
        /// <param name="message">The log message describing the error.</param>
        /// <param name="exception">Optional exception details associated with the error.</param>
        /// <returns>A task representing the asynchronous logging operation.</returns>
        public async Task LogErrorAsync(string message, Exception? exception = null)
        {
            string fullMessage = exception is not null ? $"{message}: {exception}" : message;
            await _apiClient.SendLogAsync(BetterStackLogLevel.Error, fullMessage);
        }
    }
}
