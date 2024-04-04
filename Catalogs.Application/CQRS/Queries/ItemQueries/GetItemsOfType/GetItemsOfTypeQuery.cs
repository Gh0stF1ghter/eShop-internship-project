using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemQueries.GetItemsOfType
{
    public record GetItemsOfTypeQuery(int TypeId, LinkParameters LinkParameters, bool TrackChanges) : IRequest<(LinkResponse linkResponse, MetaData metaData)>;
}
