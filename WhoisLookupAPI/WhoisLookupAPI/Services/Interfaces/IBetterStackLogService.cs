namespace WhoisLookupAPI.Services.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines logging operations for sending logs to Better Stack.
    /// </summary>
    public interface IBetterStackLogService
    {
        /// <summary>
        /// Logs an informational message to Better Stack.
        /// </summary>
        /// <param name="message">The log message to be recorded.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogInfoAsync(string message);

        /// <summary>
        /// Logs a warning message to Better Stack.
        /// </summary>
        /// <param name="message">The warning message to be recorded.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogWarningAsync(string message);

        /// <summary>
        /// Logs an error message to Better Stack.
        /// </summary>
        /// <param name="obj">The error object containing detailed error info.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogErrorAsync(object obj);
    }
}
