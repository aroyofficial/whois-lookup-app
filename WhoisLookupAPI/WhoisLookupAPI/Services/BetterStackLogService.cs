namespace WhoisLookupAPI.Services
{
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Constants;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Provides logging functionality for sending messages to Better Stack Logtail.
    /// </summary>
    public class BetterStackLogService : IBetterStackLogService
    {
        private readonly IBetterStackAPIClient _apiClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterStackLogService"/> class.
        /// </summary>
        /// <param name="apiClient">The API client responsible for sending logs to Better Stack Logtail.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="apiClient"/> is null.</exception>
        public BetterStackLogService(IBetterStackAPIClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        /// <inheritdoc/>
        public async Task LogInfoAsync(string message)
        {
            await _apiClient.SendLogAsync(BetterStackLogLevel.Info, message);
        }

        /// <inheritdoc/>
        public async Task LogWarningAsync(string message)
        {
            await _apiClient.SendLogAsync(BetterStackLogLevel.Warning, message);
        }

        /// <inheritdoc/>
        public async Task LogErrorAsync(object obj)
        {
            string message = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            await _apiClient.SendLogAsync(BetterStackLogLevel.Error, message);
        }
    }
}
