using Baskets.API.Extensions;
using Baskets.BusinessLogic;
using ItemGrpcService;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

services.ConfigureDbSettings(builder.Configuration);

services.ConfigureMongoClient();

services.AddGrpcClient<ItemService.ItemServiceClient>(options =>
options.Address = new Uri("https://localhost:7279"));

services.ConfigureMediatR();

services.AddAutoMapper(typeof(BLLAssemblyReference));

services.AddAutoValidation();

services.AddCustomDependencies();

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

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