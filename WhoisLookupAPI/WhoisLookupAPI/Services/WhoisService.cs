namespace WhoisLookupAPI.Services
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Enumerations;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Models.Request;
    using WhoisLookupAPI.Models.Response;
    using WhoisLookupAPI.Services.Interfaces;

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
            string cacheKey = $"Whois_{request.DomainName}_{request.Type}";
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
            WhoisRecord? whoisResponse = request.Type switch
            {
                WhoisRequestType.DomainInfo => await DeserializeXmlAsync<WhoisDomainInfo>(response),
                WhoisRequestType.ContactInfo => await DeserializeXmlAsync<WhoisContactInfo>(response)
            };

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

            await _logger.LogInfoAsync("Validating request object");
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

            await _logger.LogInfoAsync("Request object validated successfully");
        }

        /// <summary>
        /// Deserializes an XML string into the specified object type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="XmlSerializationException">Thrown if deserialization fails or input is invalid.</exception>
        private async Task<T> DeserializeXmlAsync<T>(string xml)
        {
            Exception exObj = null;
            T response = default;

            if (string.IsNullOrWhiteSpace(xml))
            {
                exObj = new ArgumentNullException(nameof(xml), "XML input cannot be null or empty.");
            }
            else
            {
                try
                {
                    await _logger.LogInfoAsync("Parsing XML response");
                    XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("WhoisRecord") { Namespace = "" });
                    xml = Regex.Replace(xml, @"(\+|-)?\d{2}:\d{2}|\+0000", "Z");
                    using (StringReader reader = new StringReader(xml))
                    {
                        response = (T)serializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    exObj = new XmlSerializationException("Failed to deserialize XML into the specified object type.", ex);
                }
            }

            if (exObj != null)
            {
                await _logger.LogErrorAsync(new { Exception = exObj, XMLData = xml });
                throw exObj;
            }

            await _logger.LogInfoAsync("XML response parsed successfully");
            return response;
        }
    }
}
