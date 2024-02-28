using System.Net;

namespace Baskets.DataAccess.Entities.Exceptions
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
