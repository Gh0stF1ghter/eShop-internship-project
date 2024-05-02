using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using System.Text;

namespace Ocelot.WebHost.Extensions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureOcelot(this IServiceCollection services, IConfigurationManager configuration, IWebHostEnvironment environment)
        {
            configuration
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"ocelot.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            services.AddOcelot(configuration);
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) =>
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("IdentityApiKey", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = [
                            configuration["Jwt:Baskets"],
                            configuration["Jwt:Catalogs"],
                            configuration["Jwt:Identity"],
                        ],
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Identity"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
    }
}