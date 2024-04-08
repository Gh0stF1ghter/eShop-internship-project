namespace Catalogs.Domain.Entities.Exceptions
{
    public record GrpcExceptionResponse(string StatusCode, string Message);
}
