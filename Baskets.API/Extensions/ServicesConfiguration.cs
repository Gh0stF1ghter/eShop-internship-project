﻿using Baskets.BusinessLogic;
using Baskets.DataAccess.DBContext;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using FluentValidation;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
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

        public static void AddMessageBroker(this IServiceCollection services)
        {
            services.AddMassTransit(busCfg =>
            {
                var assembly = Assembly.GetExecutingAssembly();

                busCfg.AddConsumers(assembly);
                busCfg.UsingRabbitMq((context, busFactoryCfg) =>
                {
                    busFactoryCfg.Host("rabbitmq", "/");

                    busFactoryCfg.ConfigureEndpoints(context);
                });
            });
        }

        public static void ConfigureMongoClient(this IServiceCollection services, IConfiguration configuration) =>
            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(configuration["BasketDatabaseSettings:ConnectionString"]));

        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var migrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new DropMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            };

            services.AddHangfire(config =>
            {
                config.UseSerilogLogProvider();
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseMongoStorage(configuration["BasketDatabaseSettings:ConnectionString"],
                    configuration["BasketDatabaseSettings:DatabaseName"],
                    new MongoStorageOptions
                    {
                        MigrationOptions = migrationOptions,
                        CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
                    });
            });

            services.AddHangfireServer();
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
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200", "https://localhost:5004")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        public static void AddCustomDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMongoDbContext, MongoDbContext>();
        }
    }
}