using Baskets.DataAccess.Entities.Exceptions;
using System.Net;

namespace Identity.API.Extensions
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError("Exception {ex} caught", ex.Message);

            ExceptionResponse response = ex switch
            {
                BadRequestException => new(HttpStatusCode.BadRequest, ex.Message),
                NotFoundException => new(HttpStatusCode.NotFound, ex.Message),
                _ => new(HttpStatusCode.InternalServerError, ex.Message),
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
