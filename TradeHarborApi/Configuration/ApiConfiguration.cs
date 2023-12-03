namespace TradeHarborApi.Configuration
{
    public class ApiConfiguration : IApiConfiguration
    {
        public IConfiguration Configuration { get; set; }

        public ApiConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string SqlConnectionString => Configuration["ConnectionStrings:SqlConnectionString"];
    }
}
