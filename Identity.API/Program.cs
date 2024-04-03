using Catalogs.API.Extensions;
using Identity.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

SerilogConfiguration.ConfigureLogging();
builder.Host.UseSerilog();

services.AddIdentityDbContext(builder.Configuration);
services.AddIdentitySupport();
services.AddMapper();
services.AddAutoValidation();
services.AddDependencies();
services.ConfigureSwagger();
services.ConfigureCors();
services.AddAuthentication(builder.Configuration);

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