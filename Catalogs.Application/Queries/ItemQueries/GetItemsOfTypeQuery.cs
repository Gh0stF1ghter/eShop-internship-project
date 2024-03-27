using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public record GetItemsOfTypeQuery(int TypeId, LinkParameters LinkParameters, bool TrackChanges) : IRequest<(LinkResponse linkResponse, MetaData metaData)>;
}
