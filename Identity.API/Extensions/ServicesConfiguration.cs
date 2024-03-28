using AutoMapper;
using FluentValidation;
using Identity.BusinessLogic.Mapping;
using Identity.BusinessLogic.Services.Implementations;
using Identity.BusinessLogic.Services.Interfaces;
using Identity.BusinessLogic.Validators;
using Identity.DataAccess.Data;
using Identity.DataAccess.Entities.Models;
using Identity.DataAccess.Repositories.Implementations;
using Identity.DataAccess.Repositories.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
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

        public static void AddMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles([
                    new MappingProfile(),
                    new UserProfile()
                    ]);
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddAutoValidation(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
            services.AddFluentValidationAutoValidation();
        }

        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddMessageBroker(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host("rabbitmq");
                });
            });
        }

        public static void AddMigrations(this IApplicationBuilder builder)
        {
            var services = builder.ApplicationServices.CreateScope();

            var context = services.ServiceProvider.GetService<IdentityContext>();

            context?.Database.Migrate();
        }

        public static void ApplyMigrations(this IApplicationBuilder builder)
        {
            var services = builder.ApplicationServices.CreateScope();

            var context = services.ServiceProvider.GetService<IdentityContext>();

            context?.Database.Migrate();
        }

    }
}