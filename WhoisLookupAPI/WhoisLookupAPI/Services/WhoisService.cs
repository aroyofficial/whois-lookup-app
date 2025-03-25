namespace WhoisLookupAPI.Services
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Constants;
    using WhoisLookupAPI.Enumerations;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Models.Request;
    using WhoisLookupAPI.Models.Response;
    using WhoisLookupAPI.Services.Interfaces;
    using WhoisLookupAPI.Utilities;

    /// <summary>
    /// Provides services for retrieving and caching Whois information.
    /// </summary>
    public class WhoisService : IWhoisService
    {
        private readonly IWhoisAPIClient _apiClient;
        private readonly IRedisCacheService _cacheService;
        private readonly IBetterStackLogService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisService"/> class.
        /// </summary>
        /// <param name="apiClient">The API client used for Whois lookups.</param>
        /// <param name="cacheService">Service responsible for caching Whois responses.</param>
        /// <param name="logger">Service for logging actions and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when any required dependency is null.</exception>
        public WhoisService(IWhoisAPIClient apiClient, IRedisCacheService cacheService, IBetterStackLogService logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<WhoisRecord> GetWhoisInfo(WhoisRequest request)
        {
            await ValidateRequestAsync(request);
            string cacheKey = CacheHelper.GenerateKey(request);
            WhoisRecord cachedResponse = request.Type switch
            {
                WhoisRequestType.DomainInfo => await _cacheService.GetAsync<WhoisDomainInfo>(cacheKey),
                WhoisRequestType.ContactInfo => await _cacheService.GetAsync<WhoisContactInfo>(cacheKey)
            };

            if (cachedResponse is not null)
            {
                return cachedResponse;
            }

            string response = await _apiClient.GetWhoisInfo(request.DomainName);
            
            WhoisRecord whoisResponse = request.Type switch
            {
                WhoisRequestType.DomainInfo => WhoisJsonParser.ParseResponse<WhoisDomainInfo>(response),
                WhoisRequestType.ContactInfo => WhoisJsonParser.ParseResponse<WhoisContactInfo>(response)
            };

            // if the domain is not found then set the requested domain name in the response 
            if (whoisResponse.GetType() == typeof(WhoisErrorInfo))
            {
                ((WhoisErrorInfo)whoisResponse).DomainName = request.DomainName;
            }

            await _cacheService.SetAsync<WhoisRecord>(cacheKey, whoisResponse, TimeSpan.FromMinutes(10));
            return whoisResponse;
        }

        /// <summary>
        /// Validates the given Whois request to ensure it contains required data.
        /// </summary>
        /// <param name="request">The Whois request object.</param>
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>
        /// <exception cref="InvalidRequestDataException">Thrown if the request contains missing or invalid data.</exception>
        private async Task ValidateRequestAsync(WhoisRequest request)
        {
            Exception ex = null;

            if (request == null)
            {
                ex = new ArgumentNullException(nameof(request), "Whois request cannot be null.");
            }
            else if (string.IsNullOrWhiteSpace(request.DomainName))
            {
                ex = new InvalidRequestDataException("Domain name is required. It cannot be null or empty.");
            }
            else if (!Enum.IsDefined(typeof(WhoisRequestType), request.Type))
            {
                ex = new InvalidRequestDataException("Invalid request type. Supported types are: DomainInfo, ContactInfo.");
            }

            if (ex != null)
            {
                await _logger.LogErrorAsync(new { Exception = ex, Request = request });
                throw ex;
            }
        }
    }
}
