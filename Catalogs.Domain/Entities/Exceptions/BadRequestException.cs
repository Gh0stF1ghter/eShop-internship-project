namespace Catalogs.Domain.Entities.Exceptions
{
    public class BadRequestException(string message) : Exception(message);
}
