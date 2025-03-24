namespace WhoisLookupAPI.ApiClients.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the Better Stack API Client.
    /// </summary>
    public interface IBetterStackApiClient
    {
        /// <summary>
        /// Sends a log entry to Better Stack Logtail.
        /// </summary>
        /// <param name="level">The log level (info, warning, error).</param>
        /// <param name="message">The log message.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendLogAsync(string level, string message);
    }
}
