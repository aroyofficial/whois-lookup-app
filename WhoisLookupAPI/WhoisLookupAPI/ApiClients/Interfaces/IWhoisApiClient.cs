namespace WhoisLookupAPI.ApiClients.Interfaces
{
    using System.Threading.Tasks;
    using WhoisLookupAPI.Exceptions;

    /// <summary>
    /// Defines the contract for a Whois API client to retrieve domain information.
    /// </summary>
    public interface IWhoisApiClient
    {
        /// <summary>
        /// Retrieves Whois information for the specified domain.
        /// </summary>
        /// <param name="domainName">The domain name to fetch Whois information for.</param>
        /// <returns>A task representing the asynchronous operation, containing a JSON string with the Whois information.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="domainName"/> is null or empty.</exception>
        /// <exception cref="WhoisApiException">
        /// Thrown if the API request fails due to network issues, rate limits, or other errors.
        /// </exception>
        Task<string> GetWhoisInfo(string domainName);
    }
}
