using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddAuthentication(options =>
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
                                builder.Configuration["Jwt:Baskets"],
                                builder.Configuration["Jwt:Catalogs"],
                                builder.Configuration["Jwt:Identity"],
                            ],
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Identity"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                            ClockSkew = TimeSpan.Zero
                        };
                    });

builder.Services.AddOcelot(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication().UseOcelot().Wait();

app.Run();