using FluentValidation;
using Identity.BusinessLogic.Services;
using Identity.BusinessLogic.Validators;
using Identity.DataAccess.Data;
using Identity.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;

namespace Identity.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Identity.DataAccess")));

        public static void AddIdentitySupport(this IServiceCollection services) =>
            services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) =>
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                            ClockSkew = TimeSpan.Zero
                        };
                    });

        public static void AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            services.AddFluentValidationAutoValidation();
        }

        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthenticationService, EShopAuthenticationService>();
        }
    }
}
