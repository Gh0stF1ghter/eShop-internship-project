using Catalogs.Domain.Interfaces;
using Catalogs.Infrastructure;
using Catalogs.Infrastructure.Context;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Catalogs.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void CongigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        public static void ConfigureMediatR(this IServiceCollection services) =>
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

        public static void AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);
            services.AddFluentValidationAutoValidation();
        }

        public static void AddUnitOfWork(this IServiceCollection services) =>
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
    }
}
