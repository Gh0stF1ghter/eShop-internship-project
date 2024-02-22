using Catalogs.Domain.RequestFeatures;
using MediatR;
using System.Dynamic;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemsOfTypeQuery(int TypeId, ItemParameters ItemParameters, bool TrackChanges) : IRequest<(IEnumerable<ExpandoObject> items, MetaData metaData)>;
}
