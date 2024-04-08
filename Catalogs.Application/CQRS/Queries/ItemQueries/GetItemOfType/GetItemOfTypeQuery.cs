using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemQueries.GetItemOfType
{
    public sealed record GetItemOfTypeQuery(int Id, int TypeId, bool TrackChanges) : IRequest<ItemDto>;
}
