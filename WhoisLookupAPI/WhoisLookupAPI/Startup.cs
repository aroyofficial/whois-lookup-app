namespace WhoisLookupAPI
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.RateLimiting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using StackExchange.Redis;
    using System;
    using System.Net.Http.Headers;
    using System.Threading.RateLimiting;
    using WhoisLookupAPI.ApiClients;
    using WhoisLookupAPI.ApiClients.Interfaces;
    using WhoisLookupAPI.Configurations;
    using WhoisLookupAPI.Extensions;
    using WhoisLookupAPI.Middlewares;
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
            IConfigurationRoot builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Configuration = builder;
        }

        /// <summary>
        /// Configures services and dependency injection for the application.
        /// </summary>
        /// <param name="services">The service collection to add dependencies to.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Load configuration for Whois API
            WhoisAPIConfiguration whoisApiConfiguration = new WhoisAPIConfiguration();
            Configuration.GetSection("WhoisAPI").Bind(whoisApiConfiguration);
            whoisApiConfiguration = ConfigurationResolver.Resolve(whoisApiConfiguration);
            services.AddSingleton(whoisApiConfiguration);

            // Load configuration for Redis
            RedisConfiguration redisConfiguration = new RedisConfiguration();
            Configuration.GetSection("Redis").Bind(redisConfiguration);
            redisConfiguration = ConfigurationResolver.Resolve(redisConfiguration);
            services.AddSingleton(redisConfiguration);

            // Load configuration for Better Stack
            BetterStackConfiguration betterStackConfiguration = new BetterStackConfiguration();
            Configuration.GetSection("BetterStack").Bind(betterStackConfiguration);
            betterStackConfiguration = ConfigurationResolver.Resolve(betterStackConfiguration);
            services.AddSingleton(betterStackConfiguration);

            // Register core services
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddGlobalExceptionHandler();
            // Add rate limiting policy
            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("standard", policy =>
                {
                    policy.PermitLimit = 5;
                    policy.Window = TimeSpan.FromSeconds(60);
                    policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    policy.QueueLimit = 2;
                });
            });

            // Register application services
            services.AddScoped<IWhoisService, WhoisService>();
            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            services.AddSingleton<IBetterStackLogService, BetterStackLogService>();

            // Register Redis connection multiplexer
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                RedisConfiguration redisConfig = sp.GetRequiredService<IOptions<RedisConfiguration>>().Value;
                return ConnectionMultiplexer.Connect(new ConfigurationOptions()
                {
                    EndPoints = { redisConfig.Endpoint },
                    Password = redisConfig.Password,
                    AbortOnConnectFail = false,
                    ConnectRetry = 3,
                    SyncTimeout = 5000,
                    AsyncTimeout = 5000
                });
            });

            // Register HttpClients
            services.AddHttpClient<IWhoisAPIClient, WhoisAPIClient>("WhoisApiClient", client =>
            {
                WhoisAPIConfiguration whoisConfig = Configuration.GetSection("WhoisAPI").Get<WhoisAPIConfiguration>();
                client.BaseAddress = new Uri(whoisConfig.BaseURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", whoisConfig.APIKey);
            });

            services.AddHttpClient<IBetterStackAPIClient, BetterStackAPIClient>("BetterStackApiClient", client =>
            {
                BetterStackConfiguration betterStackConfig = Configuration.GetSection("BetterStack").Get<BetterStackConfiguration>();
                client.BaseAddress = new Uri(betterStackConfig.IngestionURL);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", betterStackConfig.APIToken);
            });
        }

        /// <summary>
        /// Configures the HTTP request pipeline and middleware components.
        /// </summary>
        /// <param name="app">The application builder to configure middleware.</param>
        /// <param name="env">The hosting environment context.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            }

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseRateLimiter();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
