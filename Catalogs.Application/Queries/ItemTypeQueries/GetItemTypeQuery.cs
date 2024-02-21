using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetItemTypeQuery(int Id, bool TrackChanges) : IRequest<ItemTypeDto>;
}
