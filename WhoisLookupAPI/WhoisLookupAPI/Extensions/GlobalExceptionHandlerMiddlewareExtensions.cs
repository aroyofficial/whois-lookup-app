namespace WhoisLookupAPI.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using WhoisLookupAPI.Middlewares;

    /// <summary>
    /// Extension methods for registering and using the <see cref="GlobalExceptionHandlerMiddleware"/>.
    /// </summary>
    public static class GlobalExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Adds the global exception handler middleware to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            return services.AddTransient<GlobalExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// Uses the global exception handler middleware in the application pipeline.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
