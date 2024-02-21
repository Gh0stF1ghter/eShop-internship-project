using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetBrandsQuery(bool TrackChanges) : IRequest<IEnumerable<BrandDto>>;
}
