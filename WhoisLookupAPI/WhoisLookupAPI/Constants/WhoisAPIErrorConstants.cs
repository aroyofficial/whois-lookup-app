namespace WhoisLookupAPI.Constants
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines constant error constants returned by the Whois API and respective messages.
    /// </summary>
    public static class WhoisAPIErrorConstants
    {
        /// <summary>
        /// Dictionary containing error codes as keys and their corresponding error messages as values.
        /// </summary>
        public static readonly Dictionary<string, string> ErrorDictionary = new Dictionary<string, string>
        {
            { WHOIS_01, "An unspecified error occurred." },
            { WHOIS_02, "User is not logged in." },
            { WHOIS_03, "Unable to retrieve whois record for the specified domain name." },
            { WHOIS_04, "An unspecified error occurred." },
            { DB_01, "Authentication cannot complete due to a database error." },
            { DB_02, "Exception occurred in getUser method." },
            { DB_03, "API key is missing." },
            { DB_04, "API key is not found." },
            { DB_05, "Exception occurred in getApiKey() while retrieving entity." },
            { DB_06, "API key is not found." },
            { DB_07, "Exception occurred in getApiKey() while retrieving entity." },
            { AUTHENTICATE_01, "Queries available for the user are limited; please refill." },
            { AUTHENTICATE_02, "Queries available for the IP are limited; please refill." },
            { AUTHENTICATE_03, "Username or password is missing." },
            { AUTHENTICATE_04, "API key parameters are missing." },
            { AUTHENTICATE_05, "Access restricted due to subscription limitation." },
            { AUTHENTICATE_06, "You are limited to 30 queries per second. The request is rejected." },
            { AUTHENTICATE_07, "Request timeout." },
            { AUTHENTICATE_08, "Timestamp is in the future." },
            { AUTHENTICATE_09, "Unknown error occurred." },
            { AUTHENTICATE_10, "User account status issue." },
            { AUTHENTICATE_11, "Token is missing." },
            { AUTHENTICATE_12, "Decoding token failed." },
            { AUTHENTICATE_13, "Token has expired." },
            { AUTHENTICATE_14, "Captcha authentication failed." },
            { AUTHENTICATE_15, "Cannot retrieve IP quota." },
            { AUTHENTICATE_16, "Username is missing." },
            { AUTHENTICATE_17, "Password is missing." },
            { AUTHENTICATE_18, "Invalid username or password." },
            { API_KEY_01, "API key is disabled." },
            { API_KEY_02, "Timestamp is in the future." },
            { API_KEY_03, "Request timeout." },
            { API_KEY_04, "You are not authorized." },
            { API_KEY_05, "API key authentication failed." },
            { DNS_01, "Invalid DNS type specified." },
            { DNS_02, "General DNS error occurred." },
            { EMAIL_VERIFY_01, "Email verification error occurred." },
            { HISTORIC_WHOIS_01, "Historic whois error occurred." }
        };

        /// <summary>
        /// General error message.
        /// </summary>
        public const string WHOIS_01 = "WHOIS_01";

        /// <summary>
        /// User is not logged in.
        /// </summary>
        public const string WHOIS_02 = "WHOIS_02";

        /// <summary>
        /// Unable to retrieve whois record for the specified domain name.
        /// </summary>
        public const string WHOIS_03 = "WHOIS_03";

        /// <summary>
        /// General error message.
        /// </summary>
        public const string WHOIS_04 = "WHOIS_04";

        /// <summary>
        /// Authentication cannot complete due to database error.
        /// </summary>
        public const string DB_01 = "DB_01";

        /// <summary>
        /// Exception in getUser method.
        /// </summary>
        public const string DB_02 = "DB_02";

        /// <summary>
        /// API key is missing.
        /// </summary>
        public const string DB_03 = "DB_03";

        /// <summary>
        /// API key is not found.
        /// </summary>
        public const string DB_04 = "DB_04";

        /// <summary>
        /// Exception in getApiKey() while getting Entity.
        /// </summary>
        public const string DB_05 = "DB_05";

        /// <summary>
        /// API key is not found.
        /// </summary>
        public const string DB_06 = "DB_06";

        /// <summary>
        /// Exception in getApiKey() while getting Entity.
        /// </summary>
        public const string DB_07 = "DB_07";

        /// <summary>
        /// Queries available for the user are limited; please refill.
        /// </summary>
        public const string AUTHENTICATE_01 = "AUTHENTICATE_01";

        /// <summary>
        /// Queries available for the IP are limited; please refill.
        /// </summary>
        public const string AUTHENTICATE_02 = "AUTHENTICATE_02";

        /// <summary>
        /// Username or password is missing.
        /// </summary>
        public const string AUTHENTICATE_03 = "AUTHENTICATE_03";

        /// <summary>
        /// API key parameters are missing.
        /// </summary>
        public const string AUTHENTICATE_04 = "AUTHENTICATE_04";

        /// <summary>
        /// Access restricted due to subscription limitation.
        /// </summary>
        public const string AUTHENTICATE_05 = "AUTHENTICATE_05";

        /// <summary>
        /// You are limited to 30 queries per second. The request is rejected.
        /// </summary>
        public const string AUTHENTICATE_06 = "AUTHENTICATE_06";

        /// <summary>
        /// Request timeout.
        /// </summary>
        public const string AUTHENTICATE_07 = "AUTHENTICATE_07";

        /// <summary>
        /// Timestamp in the future.
        /// </summary>
        public const string AUTHENTICATE_08 = "AUTHENTICATE_08";

        /// <summary>
        /// Unknown error.
        /// </summary>
        public const string AUTHENTICATE_09 = "AUTHENTICATE_09";

        /// <summary>
        /// User account status issue.
        /// </summary>
        public const string AUTHENTICATE_10 = "AUTHENTICATE_10";

        /// <summary>
        /// Token is missing.
        /// </summary>
        public const string AUTHENTICATE_11 = "AUTHENTICATE_11";

        /// <summary>
        /// Decoding token failed.
        /// </summary>
        public const string AUTHENTICATE_12 = "AUTHENTICATE_12";

        /// <summary>
        /// Token has expired.
        /// </summary>
        public const string AUTHENTICATE_13 = "AUTHENTICATE_13";

        /// <summary>
        /// Captcha authentication failed.
        /// </summary>
        public const string AUTHENTICATE_14 = "AUTHENTICATE_14";

        /// <summary>
        /// Cannot get IP quota.
        /// </summary>
        public const string AUTHENTICATE_15 = "AUTHENTICATE_15";

        /// <summary>
        /// Username is missing.
        /// </summary>
        public const string AUTHENTICATE_16 = "AUTHENTICATE_16";

        /// <summary>
        /// Password is missing.
        /// </summary>
        public const string AUTHENTICATE_17 = "AUTHENTICATE_17";

        /// <summary>
        /// Invalid username or password.
        /// </summary>
        public const string AUTHENTICATE_18 = "AUTHENTICATE_18";

        /// <summary>
        /// API key is disabled.
        /// </summary>
        public const string API_KEY_01 = "API_KEY_01";

        /// <summary>
        /// Timestamp in the future.
        /// </summary>
        public const string API_KEY_02 = "API_KEY_02";

        /// <summary>
        /// Request timeout.
        /// </summary>
        public const string API_KEY_03 = "API_KEY_03";

        /// <summary>
        /// You are not authorized.
        /// </summary>
        public const string API_KEY_04 = "API_KEY_04";

        /// <summary>
        /// API key authentication failed.
        /// </summary>
        public const string API_KEY_05 = "API_KEY_05";

        /// <summary>
        /// Invalid DNS type specified.
        /// </summary>
        public const string DNS_01 = "DNS_01";

        /// <summary>
        /// General DNS error message.
        /// </summary>
        public const string DNS_02 = "DNS_02";

        /// <summary>
        /// Email verification error.
        /// </summary>
        public const string EMAIL_VERIFY_01 = "EMAIL_VERIFY_01";

        /// <summary>
        /// Historic WHOIS error.
        /// </summary>
        public const string HISTORIC_WHOIS_01 = "HISTORIC_WHOIS_01";
    }

}
