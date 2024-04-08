using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemTypeQueries.GetItemType
{
    public sealed record GetItemTypeQuery(int Id, bool TrackChanges) : IRequest<ItemTypeDto>;
}
