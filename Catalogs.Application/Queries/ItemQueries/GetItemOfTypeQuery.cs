using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetItemOfTypeQuery(int Id, int TypeId, bool TrackChanges) : IRequest<ItemDto>;
}
