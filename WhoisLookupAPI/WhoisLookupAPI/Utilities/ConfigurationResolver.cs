namespace WhoisLookupAPI.Utilities
{
    using System;
    using System.Reflection;
    using WhoisLookupAPI.Exceptions;

    /// <summary>
    /// Utility class for resolving configuration values from environment variables.
    /// </summary>
    public class ConfigurationResolver
    {
        /// <summary>
        /// Checks if an environment variable exists.
        /// </summary>
        private static readonly Func<string, bool> DoesEnvVariableExist = key =>
            !string.IsNullOrEmpty(key) && Environment.GetEnvironmentVariable(key) != null;

        /// <summary>
        /// Resolves configuration properties by replacing values with corresponding environment variables, if available.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object.</typeparam>
        /// <param name="configuration">The configuration object to resolve.</param>
        /// <returns>The updated configuration object with resolved environment variables.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the configuration object is null.</exception>
        /// <exception cref="ConfigurationResolutionException">Thrown if a property cannot be set or another issue occurs.</exception>
        public static T Resolve<T>(T configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration object cannot be null.");
            }

            try
            {
                foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!prop.CanWrite) continue; // Skip read-only properties

                    var value = prop.GetValue(configuration)?.ToString();
                    if (!string.IsNullOrEmpty(value) && DoesEnvVariableExist(value))
                    {
                        var envValue = Environment.GetEnvironmentVariable(value);
                        if (envValue != null)
                        {
                            try
                            {
                                prop.SetValue(configuration, Convert.ChangeType(envValue, prop.PropertyType));
                            }
                            catch (Exception ex)
                            {
                                throw new ConfigurationResolutionException($"Failed to set property '{prop.Name}' with environment variable value.", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ConfigurationResolutionException("An error occurred while resolving environment variables.", ex);
            }

            return configuration;
        }
    }
}
