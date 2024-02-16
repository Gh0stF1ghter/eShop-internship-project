using System.Net;

namespace Identity.DataAccess.Entities.Exceptions
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
