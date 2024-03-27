using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.BrandQueries
{
    public sealed record GetBrandsQuery(bool TrackChanges) : IRequest<IEnumerable<BrandDto>>;
}
