using Catalogs.API.Extensions;
using Serilog;
using Catalogs.Application;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

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
