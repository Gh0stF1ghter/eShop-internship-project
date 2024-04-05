namespace Catalogs.Domain.Entities.Exceptions
{
    public class AlreadyExistsException(string message) : Exception(message);
}
