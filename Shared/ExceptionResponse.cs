using System.Net;

namespace Shared
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
