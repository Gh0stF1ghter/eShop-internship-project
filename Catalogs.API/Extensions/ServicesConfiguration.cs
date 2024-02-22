﻿using Catalogs.API.ActionFilters;
using Catalogs.API.Utility;
using Catalogs.Application.DataShaping;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Interfaces;
using Catalogs.Infrastructure;
using Catalogs.Infrastructure.Context;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        public static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDataShaper<ItemDto>, DataShaper<ItemDto>>();
            services.AddScoped<ValidateMediaTypeAttribute>();
            services.AddScoped<IItemLinks, ItemLinks>();
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
    }
}
