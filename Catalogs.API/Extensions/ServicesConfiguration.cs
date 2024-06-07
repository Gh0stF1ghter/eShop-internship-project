using Catalogs.API.ActionFilters;
using Catalogs.API.Utility;
using Catalogs.Application.DataShaping;
using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Interfaces;
using Catalogs.Infrastructure;
using Catalogs.Infrastructure.Context;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

namespace Catalogs.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void CongigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Catalogs.Infrastructure")));

        public static void ConfigureMediatR(this IServiceCollection services) =>
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

        public static void AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);
            services.AddFluentValidationAutoValidation();
        }

        public static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDataShaper<ItemDto>, DataShaper<ItemDto>>();
            services.AddScoped<ValidateMediaTypeAttribute>();
            services.AddScoped<IItemLinks<ItemDto>, ItemLinks>();
        }

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Pagination"));
            });

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters
                                                                .OfType<SystemTextJsonOutputFormatter>()
                                                                .FirstOrDefault();

                systemTextJsonOutputFormatter?.SupportedMediaTypes
                        .Add("application/vnd.eShopTest.hateoas+json");
            });
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) =>
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Identity"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

        public static void ApplyMigrations(this IApplicationBuilder builder)
        {
            var services = builder.ApplicationServices.CreateScope();

            var context = services.ServiceProvider.GetService<CatalogContext>();

            context?.Database.Migrate();
        }
    }
}