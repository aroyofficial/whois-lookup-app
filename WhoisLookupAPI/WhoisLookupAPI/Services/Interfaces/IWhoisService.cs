namespace WhoisLookupAPI.Services.Interfaces
{
    using System.Threading.Tasks;
    using WhoisLookupAPI.Models.Request;
    using WhoisLookupAPI.Models.Response;

    /// <summary>
    /// Interface for the Whois service to retrieve domain and contact information.
    /// </summary>
    public interface IWhoisService
    {
        /// <summary>
        /// Retrieves Whois information based on the request.
        /// </summary>
        /// <param name="request">The Whois request containing domain details.</param>
        /// <returns>A <see cref="WhoisRecord"/> object containing the Whois information.</returns>
        Task<WhoisRecord> GetWhoisInfo(WhoisRequest request);
    }
}
