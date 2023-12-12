namespace TradeHarborApi.Configuration
{
    public class ApiConfiguration : IApiConfiguration
    {
        private IConfiguration Configuration { get; set; }

        public ApiConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string SqlConnectionString => GetSqlConnectionString();

        private string GetSqlConnectionString()
        {
            var sqlConnectionString = Configuration["ConnectionStrings:SqlConnectionString"];
            if (sqlConnectionString != null)
            {
                return sqlConnectionString;
            }
            else
            {
                throw new InvalidOperationException("Sql connection string is null.");
            }
        }
    }
}
