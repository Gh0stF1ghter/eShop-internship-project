using Ocelot.Middleware;
using Ocelot.WebHost.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.ConfigureOcelot(builder.Configuration, builder.Environment);
services.AddAuthentication(builder.Configuration);
services.ConfigureCors();

services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication().UseOcelot().Wait();

app.Run();