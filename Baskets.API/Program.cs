using Baskets.BusinessLogic;
using Baskets.DataAccess.Entities.Models;
using Identity.API.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.Configure<BasketDatabaseSettings>(
    builder.Configuration.GetSection(nameof(BasketDatabaseSettings)));

services.AddAutoMapper(typeof(BLLAssemblyReference));
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(BLLAssemblyReference).Assembly));

services.AddSingleton<IMongoClient>(_ =>
{
    var settings = new MongoClientSettings()
    {
        Scheme = ConnectionStringScheme.MongoDB,
        Server = new MongoServerAddress("localhost", 27017)
    };

    return new MongoClient(settings);
});

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
