namespace WhoisLookupAPI
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.RateLimiting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using StackExchange.Redis;
    using System;
    using System.Net.Http.Headers;
    using System.Threading.RateLimiting;
    using WhoisLookupAPI.ApiClients;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Configurations;
    using WhoisLookupAPI.Services;
    using WhoisLookupAPI.Services.Interfaces;
    using WhoisLookupAPI.Utilities;

    /// <summary>
    /// Represents the startup configuration for the application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets the application configuration properties.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration settings.</param>
        public Startup(IConfiguration configuration)
        {
            // Load and bind configuration from appsettings.json, environment-specific settings, and environment variables
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Ensure correct base path
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // Load base configuration
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true) // Load environment-specific settings
                .AddEnvironmentVariables() // Load environment variables
                .Build();

            Configuration = configBuilder;
        }

        /// <summary>
        /// Configures services and dependency injection for the application.
        /// </summary>
        /// <param name="services">The service collection to add dependencies to.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add essential services to the dependency injection container
            services.AddControllers(); // Enables API controllers
            services.AddEndpointsApiExplorer(); // Enables API endpoint exploration
            services.AddSwaggerGen(); // Adds Swagger for API documentation

            // Add rate limit policy
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", policy =>
                {
                    policy.PermitLimit = 5; // Allow up to 5 requests per 60 seconds
                    policy.Window = TimeSpan.FromSeconds(60); // Reset every 60 seconds
                    policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    policy.QueueLimit = 2; // Allow 2 extra requests to be queued
                });
            });

            // Load and resolve configuration values
            var whoisConfig = new WhoisApiConfiguration();
            var redisConfig = new RedisConfiguration();
            var betterStackConfig = new BetterStackConfiguration();
            Configuration.GetSection("WhoisAPI").Bind(whoisConfig);
            Configuration.GetSection("Redis").Bind(redisConfig);
            Configuration.GetSection("BetterStack").Bind(betterStackConfig);

            // Resolve environment variables for configuration properties
            whoisConfig = ConfigurationResolver.Resolve(whoisConfig);
            redisConfig = ConfigurationResolver.Resolve(redisConfig);
            betterStackConfig = ConfigurationResolver.Resolve(betterStackConfig);

            // Register Whois service for domain lookup functionality
            services.AddScoped<IWhoisService, WhoisService>();

            // Register Redis caching service
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            // Register Better Stack logging service
            services.AddSingleton<IBetterStackLogService, BetterStackLogService>();

            // Configure and register HttpClient for the Whois API client
            services.AddHttpClient<IWhoisApiClient, WhoisApiClient>("WhoisApiClient", client =>
            {
                client.BaseAddress = new Uri(whoisConfig.BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whoisConfig.APIKey);
            });

            // Configure and register HttpClient for the Better Stack API client
            services.AddHttpClient<IBetterStackApiClient, BetterStackApiClient>("BetterStackApiClient", client =>
            {
                client.BaseAddress = new Uri(betterStackConfig.IngestionURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", betterStackConfig.APIToken);
            });

            // Configure and register Redis connection multiplexer
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                EndPoints = { redisConfig.Endpoint },
                Password = redisConfig.Password,
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                SyncTimeout = 5000,
                AsyncTimeout = 5000
            }));
        }

        /// <summary>
        /// Configures the HTTP request pipeline and middleware components.
        /// </summary>
        /// <param name="app">The application builder to configure middleware.</param>
        /// <param name="env">The hosting environment context.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Enables developer exception pages and Swagger UI in development
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure middleware pipeline
            app.UseRouting(); // Enables request routing
            app.UseRateLimiter(); // Enable rate limiting

            app.UseEndpoints(endpoints =>
            {
                // Maps API controllers to route endpoints
                endpoints.MapControllers();
            });
        }
    }
}
