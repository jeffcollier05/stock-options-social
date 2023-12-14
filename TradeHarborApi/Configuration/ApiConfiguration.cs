namespace TradeHarborApi.Configuration
{
    /// <summary>
    /// Implementation of the IApiConfiguration interface providing access to API-related configuration settings.
    /// </summary>
    public class ApiConfiguration : IApiConfiguration
    {
        private IConfiguration Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the ApiConfiguration class.
        /// </summary>
        /// <param name="configuration">The IConfiguration instance.</param>
        public ApiConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string SqlConnectionString => GetSqlConnectionString();

        /// <summary>
        /// Retrieves the SQL Server connection string from the application configuration.
        /// </summary>
        /// <returns>The SQL Server connection string.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the SQL Server connection string is null or missing in the configuration.
        /// </exception>
        private string GetSqlConnectionString()
        {
            var sqlConnectionString = Configuration["ConnectionStrings:SqlConnectionString"];

            if (sqlConnectionString is not null)
            {
                return sqlConnectionString;
            }

            throw new InvalidOperationException("SQL connection string is null.");
        }
    }
}
