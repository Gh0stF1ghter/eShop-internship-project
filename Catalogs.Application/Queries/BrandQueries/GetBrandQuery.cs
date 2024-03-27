using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.BrandQueries
{
    public sealed record GetBrandQuery(int Id, bool TrackChanges) : IRequest<BrandDto>;
}
