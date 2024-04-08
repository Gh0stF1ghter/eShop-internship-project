using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.BrandQueries.GetBrand
{
    public sealed record GetBrandQuery(int Id, bool TrackChanges) : IRequest<BrandDto>;
}
