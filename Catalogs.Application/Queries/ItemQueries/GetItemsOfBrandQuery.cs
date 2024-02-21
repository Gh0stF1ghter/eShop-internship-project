using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemsOfBrandQuery(int BrandId, bool TrackChanges) : IRequest<IEnumerable<ItemDto>>;
}
