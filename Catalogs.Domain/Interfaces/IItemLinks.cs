using Catalogs.Domain.Entities.LinkModels;
using Microsoft.AspNetCore.Http;

namespace Catalogs.Domain.Interfaces
{
    public interface IItemLinks<T>
    {
        LinkResponse TryGenerateLinks(IEnumerable<T> items, string fields, int typeId, HttpContext httpContext);
    }
}
