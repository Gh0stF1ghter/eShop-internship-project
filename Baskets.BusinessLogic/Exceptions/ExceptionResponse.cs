using System.Net;

namespace Baskets.BusinessLogic.Exceptions
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
