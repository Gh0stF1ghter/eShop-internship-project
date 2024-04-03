using Catalogs.API.Extensions;
using Identity.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

SerilogConfiguration.ConfigureLogging();
builder.Host.UseSerilog();

services.AddIdentityDbContext(builder.Configuration);

services.AddIdentitySupport();

services.AddAuthentication(builder.Configuration);

services.AddMapper();

services.AddAutoValidation();

services.AddDependencies();

services.AddControllers();

services.AddEndpointsApiExplorer();

services.ConfigureSwagger();

var app = builder.Build();

app.ApplyMigrations();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.UseAuthorization();

app.Run();