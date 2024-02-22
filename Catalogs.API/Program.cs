using Catalogs.API.Extensions;
using Identity.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

services.CongigureSqlContext(builder.Configuration);

services.AddUnitOfWork();
services.ConfigureMediatR();
services.AddAutoValidation();
services.AddAutoMapper(typeof(Program));

services.ConfigureCors();

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
