using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.LinkModels;
using Microsoft.AspNetCore.Http;

namespace Catalogs.Domain.Interfaces
{
    public interface IItemLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<ItemDto> itemsDtos, string fields, int typeId, HttpContext httpContext);
    }
}
