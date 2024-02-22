using Catalogs.Domain.RequestFeatures;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemsOfTypeQuery(int TypeId, ItemParameters ItemParameters, bool TrackChanges) : IRequest<(IEnumerable<ItemDto> items, MetaData metaData)>;
}
