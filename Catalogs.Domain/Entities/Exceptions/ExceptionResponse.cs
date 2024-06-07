using System.Net;

namespace Catalogs.Domain.Entities.Exceptions
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
