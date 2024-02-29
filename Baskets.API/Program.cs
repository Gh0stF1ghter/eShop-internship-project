using Baskets.API.Extensions;
using Baskets.BusinessLogic;
using Baskets.DataAccess.Entities.Models;
using Baskets.DataAccess.UnitOfWork;
using FluentValidation;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

services.ConfigureDbSettings(builder.Configuration);

services.ConfigureMongoClient();

services.ConfigureMediatR();

services.AddAutoMapper(typeof(BLLAssemblyReference));

services.AddAutoValidation();

services.AddCustomDependencies();

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
