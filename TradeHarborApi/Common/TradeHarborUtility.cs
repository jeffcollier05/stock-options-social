using Microsoft.IdentityModel.Tokens;

namespace TradeHarborApi.Common
{
    public static class TradeHarborUtility
    {
        /// <summary>
        /// Finds the value of a specific configuration key within the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration interface providing access to application settings.</param>
        /// <param name="claimKey">The key for the configuration value to find.</param>
        /// <returns>The value of the specified configuration key.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the configuration value for the specified key is missing.
        /// </exception>
        internal static string FindConfigurationValue(IConfiguration configuration, string claimKey)
        {
            var claimValue = configuration.GetSection(key: claimKey)?.Value;
            if (claimValue == null)
            {
                throw new InvalidOperationException($"Missing the configuration value for the key {claimKey}.");
            }

            return claimValue;
        }

        /// <summary>
        /// Finds the values of a specific configuration key within the provided configuration.
        /// </summary>
        /// <param name="configuration">The configuration interface providing access to application settings.</param>
        /// <param name="claimKey">The key for the configuration value to find.</param>
        /// <returns>The values of the specified configuration key.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the configuration value for the specified key is missing.
        /// </exception>
        internal static string[] FindConfigurationValues(IConfiguration configuration, string claimKey)
        {
            var claimValues = configuration.GetSection(key: claimKey).Get<string[]>();
            if (claimValues == null || claimValues.IsNullOrEmpty())
            {
                throw new InvalidOperationException($"Missing the configuration value for the key {claimKey}.");
            }

            return claimValues;
        }
    }
}
