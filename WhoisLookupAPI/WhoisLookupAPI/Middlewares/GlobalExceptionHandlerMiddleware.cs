namespace WhoisLookupAPI.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using WhoisLookupAPI.Models.Response;

    /// <summary>
    /// Middleware to handle global exceptions and return structured error responses.
    /// Implements <see cref="IMiddleware"/> for better dependency injection support.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="logger">Logger instance for logging errors.</param>
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the middleware to handle exceptions globally.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <param name="next">The next middleware delegate in the pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the exception by generating a structured error response.
        /// </summary>
        /// <param name="context">The HTTP context of the current request.</param>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            object errorResponse = new WhoisErrorResponse()
            {
                ErrorCode = Enumerations.ErrorCode.InternalServerError,
                ErrorMessage = "An unexpected error occurred. Please try again later.",
                ErrorDetails = exception.Message // Can be omitted in production for security reasons
            };

            string jsonResponse = JsonSerializer.Serialize(errorResponse);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}