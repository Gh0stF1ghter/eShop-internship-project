using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemTypeQueries
{
    public sealed record GetItemTypesQuery(bool TrackChanges) : IRequest<IEnumerable<ItemTypeDto>>;
}
