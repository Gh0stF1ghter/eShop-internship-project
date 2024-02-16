using System.Net;

namespace Identity.DataAccess.Exceptions
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
