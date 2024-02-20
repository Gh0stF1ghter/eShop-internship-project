using Catalogs.Domain.Interfaces;
using Catalogs.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Catalogs.Application.AssemblyReference).Assembly));
services.AddAutoMapper(typeof(Program));

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

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
