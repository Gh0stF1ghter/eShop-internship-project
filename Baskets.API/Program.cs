using Baskets.API.Extensions;
using Baskets.BusinessLogic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

services.ConfigureDbSettings(builder.Configuration);
services.ConfigureMongoClient(builder.Configuration);
services.ConfigureRedisCache(builder.Configuration);
services.ConfigureMediatR();
services.AddAutoValidation();
services.AddCustomDependencies();
services.AddAuthentication(builder.Configuration);
services.ConfigureSwagger();

services.AddAutoMapper(typeof(BLLAssemblyReference));
services.AddControllers();
services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();