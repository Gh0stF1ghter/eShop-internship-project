using Catalogs.API.Extensions;
using Catalogs.Application;
using Serilog;


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