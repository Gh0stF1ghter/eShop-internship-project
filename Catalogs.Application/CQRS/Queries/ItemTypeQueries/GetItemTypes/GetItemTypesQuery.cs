using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemTypeQueries.GetItemTypes
{
    public sealed record GetItemTypesQuery(bool TrackChanges) : IRequest<IEnumerable<ItemTypeDto>>;
}
