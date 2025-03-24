namespace WhoisLookupAPI.Services
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using WhoisLookupAPI.ApiClients;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Enumerations;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Models.Request;
    using WhoisLookupAPI.Models.Response;
    using WhoisLookupAPI.Services.Interfaces;

    /// <summary>
    /// Provides services for retrieving Whois information based on user requests.
    /// </summary>
    public class WhoisService : IWhoisService
    {
        private readonly IWhoisApiClient _apiClient;
        private readonly IRedisCacheService _cacheService;
        private readonly IBetterStackLogService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhoisService"/> class.
        /// </summary>
        /// <param name="apiClient">The API client used to interact with the external Whois API.</param>
        /// <exception cref="ArgumentNullException">Thrown when any dependency is null.</exception>
        public WhoisService(IWhoisApiClient apiClient, IRedisCacheService cacheService, IBetterStackLogService logger)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves Whois information based on the request type.
        /// </summary>
        /// <param name="request">The request containing the domain name and type of Whois data required.</param>
        /// <returns>
        /// A <see cref="WhoisRecord"/> object containing domain or contact information.
        /// </returns>
        /// <exception cref="InvalidRequestDataException">
        /// Thrown if the request is null, contains an invalid domain name, or an unsupported request type.
        /// </exception>
        public async Task<WhoisRecord> GetWhoisInfo(WhoisRequest request)
        {
            ValidateRequest(request);
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

            try
            {
                string response = await _apiClient.GetWhoisInfo(request.DomainName);
                WhoisRecord? whoisResponse = request.Type switch
                {
                    WhoisRequestType.DomainInfo => DeserializeXml<WhoisDomainInfo>(response) ?? throw new InvalidResponseDataException("Failed to parse Whois API response."),
                    WhoisRequestType.ContactInfo => DeserializeXml<WhoisContactInfo>(response) ?? throw new InvalidResponseDataException("Failed to parse Whois API response.")
                };

                await _cacheService.SetAsync<WhoisRecord>(cacheKey, whoisResponse, TimeSpan.FromMinutes(10));
                return whoisResponse;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Error fetching Whois data for {request.DomainName}", ex);
                throw;
            }
        }

        /// <summary>
        /// Validates the Whois request to ensure it contains valid data.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
        /// <exception cref="InvalidRequestDataException">
        /// Thrown if the domain name is missing or if the request type is invalid.
        /// </exception>
        private static void ValidateRequest(WhoisRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Whois request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.DomainName))
            {
                throw new InvalidRequestDataException("Domain name is required. It cannot be null or empty.");
            }

            if (!Enum.IsDefined(typeof(WhoisRequestType), request.Type))
            {
                throw new InvalidRequestDataException("Invalid request type. Supported types are: DomainInfo, ContactInfo.");
            }
        }

        /// <summary>
        /// Deserializes an XML string into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize into.</typeparam>
        /// <param name="xml">The XML string to deserialize.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if the XML input is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown if deserialization fails due to invalid XML.</exception>
        private static T DeserializeXml<T>(string xml)
        {
            // Ensure the XML input is valid
            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentException("XML input cannot be null or empty.", nameof(xml));

            try
            {
                // Initialize the XML serializer with the specified type and root attribute
                var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("WhoisRecord") { Namespace = "" });

                // Normalize date-time format to use 'Z' instead of time zone offsets
                xml = Regex.Replace(xml, @"(\+|-)\d{2}:\d{2}|\+0000", "Z");

                // Deserialize the XML string
                using (StringReader reader = new StringReader(xml))
                {
                    return (T)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to deserialize XML into the specified object type.", ex);
            }
        }
    }
}
