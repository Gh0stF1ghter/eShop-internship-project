using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetItemTypesQuery(bool TrackChanges) : IRequest<IEnumerable<ItemTypeDto>>;
}
