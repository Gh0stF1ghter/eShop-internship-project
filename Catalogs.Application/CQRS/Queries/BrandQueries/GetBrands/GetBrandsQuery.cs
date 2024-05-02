using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.BrandQueries.GetBrands
{
    public sealed record GetBrandsQuery(bool TrackChanges) : IRequest<IEnumerable<BrandDto>>;
}
