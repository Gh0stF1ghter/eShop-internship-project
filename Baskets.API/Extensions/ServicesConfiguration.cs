using Baskets.BusinessLogic;
using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using FluentValidation;
using MongoDB.Driver;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

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

        public static void ConfigureMongoClient(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(configuration["BasketDatabaseSettings:ConnectionString"]));

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

        public static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMongoDbContext, MongoDbContext>();
        }
    }
}