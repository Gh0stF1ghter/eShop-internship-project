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

services.AddMapper();

services.AddAutoValidation();

services.AddMessageBroker();

services.AddDependencies();

services.AddControllers();
services.AddEndpointsApiExplorer();

var app = builder.Build();

app.ApplyMigrations();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.MapControllers();

app.Run();