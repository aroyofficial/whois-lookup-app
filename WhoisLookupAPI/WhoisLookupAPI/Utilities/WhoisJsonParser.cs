namespace WhoisLookupAPI.Utilities
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Reflection;
    using WhoisLookupAPI.Constants;
    using WhoisLookupAPI.Exceptions;
    using WhoisLookupAPI.Models.Response;

    /// <summary>
    /// Utility class for parsing JSON responses from the Whois API.
    /// </summary>
    public static class WhoisJsonParser
    {
        /// <summary>
        /// Parses the JSON response from the Whois API and converts it into a <see cref="WhoisRecord"/> object.
        /// </summary>
        /// <typeparam name="T">
        /// The type to deserialize the Whois API response into. Must inherit from <see cref="WhoisRecord"/>.
        /// </typeparam>
        /// <param name="jsonResponse">The raw JSON response received from the Whois API.</param>
        /// <returns>
        /// A <see cref="WhoisRecord"/> object containing parsed domain information.
        /// If an error is found, returns a <see cref="WhoisErrorInfo"/> object instead.
        /// </returns>
        /// <exception cref="WhoisAPIResponseParsingException">
        /// Thrown when the Whois API response cannot be parsed or if a required field is missing.
        /// </exception>
        /// <exception cref="JsonReaderException">
        /// Thrown if the JSON response is malformed and cannot be parsed.
        /// </exception>
        /// <exception cref="JsonSerializationException">
        /// Thrown if JSON deserialization fails due to a mismatch with the expected structure.
        /// </exception>
        public static WhoisRecord ParseResponse<T>(string jsonResponse) where T : WhoisRecord
        {
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                throw new ArgumentException("JSON response cannot be null or empty.", nameof(jsonResponse));
            }

            try
            {
                JObject response = JObject.Parse(jsonResponse);

                // Check if the response contains an error
                WhoisErrorInfo errorResponse = GetWhoisAPIError(jsonResponse);
                if (errorResponse != null)
                {
                    return errorResponse;
                }

                if (response.TryGetValue("WhoisRecord", out JToken whoisRecordToken) && whoisRecordToken != null)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ContractResolver = new WhoisJsonContractResolver(),
                        MissingMemberHandling = MissingMemberHandling.Ignore // Prevent exceptions due to missing fields
                    };

                    JsonSerializer serializer = JsonSerializer.Create(settings);
                    return whoisRecordToken.ToObject<T>(serializer);
                }

                throw new WhoisAPIResponseParsingException("Failed to parse Whois API response: Missing 'WhoisRecord' field.", jsonResponse);
            }
            catch (JsonReaderException ex)
            {
                throw new WhoisAPIResponseParsingException("Malformed JSON response received from the Whois API.", jsonResponse, ex);
            }
            catch (JsonSerializationException ex)
            {
                throw new WhoisAPIResponseParsingException("Failed to deserialize Whois API response into the expected format.", jsonResponse, ex);
            }
            catch (Exception ex)
            {
                throw new WhoisAPIResponseParsingException("An unexpected error occurred while parsing the Whois API response.", jsonResponse, ex);
            }
        }

        /// <summary>
        /// Extracts the Whois API error from the given JSON response, if any.
        /// </summary>
        /// <param name="jsonResponse">The JSON response received from the Whois API.</param>
        /// <returns>
        /// A <see cref="WhoisErrorInfo"/> object containing the error message if an error code is found;
        /// otherwise, returns <c>null</c>.
        /// </returns>
        public static WhoisErrorInfo GetWhoisAPIError(string jsonResponse)
        {
            if (string.IsNullOrWhiteSpace(jsonResponse))
            {
                return null;
            }

            try
            {
                // Retrieve all error codes from WhoisAPIErrorConstants
                var errorCodes = typeof(WhoisAPIErrorConstants)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                    .Select(f => f.GetValue(null)?.ToString())
                    .Where(code => !string.IsNullOrEmpty(code))
                    .ToList();

                // Check if the JSON response contains any of the known error codes
                string foundErrorCode = errorCodes.FirstOrDefault(code => jsonResponse.Contains(code, StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(foundErrorCode) && WhoisAPIErrorConstants.ErrorDictionary.TryGetValue(foundErrorCode, out string errorMessage))
                {
                    return new WhoisErrorInfo { Message = errorMessage };
                }
            }
            catch (Exception ex)
            {
                throw new WhoisAPIResponseParsingException("Failed to extract error information from the Whois API response.", jsonResponse, ex);
            }

            return null;
        }
    }
}
