using Catalogs.API.Extensions;
using Catalogs.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

SerilogConfiguration.ConfigureLogging();
builder.Host.UseSerilog();

var services = builder.Services;

services.CongigureSqlContext(builder.Configuration);

services.AddCustomDependencies();

services.ConfigureMediatR();
services.AddAutoValidation();
services.AddAutoMapper(typeof(AssemblyReference));

services.ConfigureCors();

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCustomMediaTypes();

services.AddAuthentication(builder.Configuration);

var app = builder.Build();

app.ApplyMigrations();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();