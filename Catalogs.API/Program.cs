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
services.ConfigureCors();
services.ConfigureSwagger();
services.AddAuthentication(builder.Configuration);
services.AddCustomMediaTypes();

services.AddAutoMapper(typeof(AssemblyReference));
services.AddControllers();
services.AddEndpointsApiExplorer();

var app = builder.Build();

app.ApplyMigrations();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();