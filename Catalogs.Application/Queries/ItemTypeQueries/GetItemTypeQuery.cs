using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemTypeQueries
{
    public sealed record GetItemTypeQuery(int Id, bool TrackChanges) : IRequest<ItemTypeDto>;
}
