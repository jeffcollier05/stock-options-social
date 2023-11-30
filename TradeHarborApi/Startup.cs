using TradeHarborApi.Configuration;
using TradeHarborApi.Repositories;
using TradeHarborApi.Services;

namespace TradeHarborApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; set; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TradesRepository>();
            services.AddScoped<TradeService>();
            services.AddScoped<IApiConfiguration, ApiConfiguration>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();

            var corsOrigins = Configuration.GetSection("Cors:Origins").Get<string[]>();
            var corsMethods = Configuration.GetSection("Cors:Methods").Get<string[]>();
            services.AddCors(options => options.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins(corsOrigins)
                .WithMethods(corsMethods)
                .AllowAnyHeader()));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
