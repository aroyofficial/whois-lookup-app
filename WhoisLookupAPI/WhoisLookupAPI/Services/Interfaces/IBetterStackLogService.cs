namespace WhoisLookupAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines logging operations for sending logs to Better Stack.
    /// </summary>
    public interface IBetterStackLogService
    {
        /// <summary>
        /// Logs an error message to Better Stack.
        /// </summary>
        /// <param name="obj">The error object containing detailed error info.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogErrorAsync(object obj);
    }
}
