using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemsOfVendorQuery(int VendorId, bool TrackChanges) : IRequest<IEnumerable<ItemDto>>;
}
