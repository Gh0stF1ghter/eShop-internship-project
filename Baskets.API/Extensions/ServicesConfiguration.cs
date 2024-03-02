using Baskets.BusinessLogic;
using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Baskets.API.Extensions
{
    public static class ServicesConfiguration
    {
        public static void ConfigureDbSettings(this IServiceCollection services, IConfiguration configuration) =>
            services.Configure<BasketDatabaseSettings>(
                configuration.GetSection(nameof(BasketDatabaseSettings)));

        public static void ConfigureMediatR(this IServiceCollection services) =>
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BLLAssemblyReference).Assembly));
        public static void AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(BLLAssemblyReference).Assembly);
            services.AddFluentValidationAutoValidation();
        }

        public static void ConfigureMongoClient(this IServiceCollection services) =>
            services.AddSingleton<IMongoClient>(_ =>
            {
                var settings = new MongoClientSettings()
                {
                    Scheme = ConnectionStringScheme.MongoDB,
                    Server = new MongoServerAddress("localhost", 27017)
                };

                return new MongoClient(settings);
            });

        public static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMongoDbContext, MongoDbContext>();
        }
    }
}