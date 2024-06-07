using Baskets.API.Extensions;
using Baskets.API.Hubs;
using Baskets.BusinessLogic;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

services.ConfigureDbSettings(builder.Configuration);
services.ConfigureMongoClient(builder.Configuration);
services.ConfigureMediatR();
services.AddAutoValidation();
services.AddCustomDependencies();
services.ConfigureHangfire(builder.Configuration);
services.AddAuthentication(builder.Configuration);
services.ConfigureSwagger();
services.ConfigureCors();
services.AddAutoMapper(typeof(BLLAssemblyReference));

services.AddAutoValidation();

services.AddMessageBroker();

services.AddCustomDependencies();

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSignalR();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();
app.MapControllers();
app.MapHub<BasketHub>("/basket");

app.UseHangfireDashboard();

app.Run();