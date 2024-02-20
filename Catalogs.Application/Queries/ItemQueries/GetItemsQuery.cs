using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetItemsQuery(bool TrackChanges) : IRequest<IEnumerable<ItemDTO>>;
}
