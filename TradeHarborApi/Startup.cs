using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TradeHarborApi.Common;
using TradeHarborApi.Configuration;
using TradeHarborApi.Data;
using TradeHarborApi.Repositories;
using TradeHarborApi.Services;

namespace TradeHarborApi
{
    /// <summary>
    /// Configures the application's services and request processing pipeline.
    /// </summary>
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="environment">The hosting environment.</param>
        /// <param name="configuration">The configuration settings.</param>
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            Configuration = configuration;
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAppSettings(services);
            ConfigureDatabase(services);
            ConfigureDependencyInjection(services);
            ConfigureApi(services);
            ConfigureSwagger(services);
            ConfigureIdentity(services);
            ConfigureAuthentication(services);
            ConfigureCorsPolicy(services);
        }

        /// <summary>
        /// Configures application settings using the provided <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.Configure<JwtConfig>(Configuration.GetSection(key: "JwtConfig"));
        }

        /// <summary>
        /// Configures the database context for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        private void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(optionsAction: options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnectionString"));
            });
        }

        /// <summary>
        /// Configures dependency injection for various services in the application.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<SocialRepository>();
            services.AddScoped<SocialService>();
            services.AddScoped<AuthService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<AuthRepository>();
            services.AddScoped<IApiConfiguration, ApiConfiguration>();
        }

        /// <summary>
        /// Configures API-related services for the application.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        private void ConfigureApi(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
        }

        /// <summary>
        /// Configures Swagger (OpenAPI) documentation for the API.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeHarborApi", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
        }

        /// <summary>
        /// Configures the default Identity system for the application.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <remarks>
        /// This method sets up the default Identity system using <see cref="IdentityUser"/>.
        /// It configures options for user sign-in, and the entity framework store for user data.
        /// </remarks>
        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(configureOptions: options =>
                options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ApiDbContext>();
        }

        /// <summary>
        /// Configures authentication for the application using JWT Bearer authentication.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <remarks>
        /// This method sets up JWT Bearer authentication as the default authentication scheme.
        /// It configures options for token validation parameters, including issuer, audience, and expiration time.
        /// </remarks>
        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(configureOptions: options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secret = TradeHarborUtility.FindConfigurationValue(Configuration, "JwtConfig:Secret");
                var key = Encoding.ASCII.GetBytes(secret);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    // Set the valid issuer and audience values
                    ValidateIssuer = true,
                    ValidIssuer = TradeHarborUtility.FindConfigurationValue(Configuration, "JwtConfig:ValidIssuer"),
                    ValidateAudience = true,
                    ValidAudience = TradeHarborUtility.FindConfigurationValue(Configuration, "JwtConfig:ValidAudience"),

                    // Enable expiration time and lifetime validation
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });
        }

        /// <summary>
        /// Configures Cross-Origin Resource Sharing (CORS) policy for the application based on configuration settings.
        /// </summary>
        /// <param name="services">The service collection to configure.</param>
        /// <remarks>
        /// This method sets up a CORS policy allowing requests from specified origins and methods.
        /// It uses configuration values to determine allowed origins and methods.
        /// </remarks>
        private void ConfigureCorsPolicy(IServiceCollection services)
        {
            var corsOrigins = TradeHarborUtility.FindConfigurationValues(Configuration, "Cors:Origins");
            var corsMethods = TradeHarborUtility.FindConfigurationValues(Configuration, "Cors:Methods");
            services.AddCors(options => options.AddDefaultPolicy(policy =>
                policy.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins(corsOrigins)
                    .WithMethods(corsMethods)
                    .AllowAnyHeader()));
        }

        /// <summary>
        /// Configures the application's request processing pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        public void Configure(IApplicationBuilder app)
        {
            ConfigureDevelopmentEnvironment(app);
            ConfigureExceptionHandler(app);

            app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Configures the development environment-specific settings.
        /// </summary>
        /// <param name="app">The application builder.</param>
        private void ConfigureDevelopmentEnvironment(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }

        /// <summary>
        /// Configures the global exception handler middleware for the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        private void ConfigureExceptionHandler(IApplicationBuilder app)
        {
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionFeature != null)
                {
                    var exception = exceptionFeature.Error;
                    var response = new { error = exception?.Message };
                    await context.Response.WriteAsJsonAsync(response);
                }
            }));
        }
    }
}
