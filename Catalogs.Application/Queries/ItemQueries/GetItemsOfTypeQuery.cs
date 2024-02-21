using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemsOfTypeQuery(int TypeId, bool TrackChanges) : IRequest<IEnumerable<ItemDto>>;
}
