﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TradeHarborApi.Configuration;
using TradeHarborApi.Data;
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
            services.Configure<JwtConfig>(Configuration.GetSection(key: "JwtConfig"));

            services.AddDbContext<ApiDbContext>(optionsAction: options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnectionString"));
            });

            services.AddScoped<TradesRepository>();
            services.AddScoped<TradeService>();
            services.AddScoped<IApiConfiguration, ApiConfiguration>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();

            services.AddDefaultIdentity<IdentityUser>(configureOptions: options =>
                    options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApiDbContext>();

            services.AddAuthentication(configureOptions: options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration.GetSection(key: "JwtConfig:Secret").Value);
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false
                };
            });

            var corsOrigins = Configuration.GetSection(key: "Cors:Origins").Get<string[]>();
            var corsMethods = Configuration.GetSection(key: "Cors:Methods").Get<string[]>();
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

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { error = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));
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
    }
}
