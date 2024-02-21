using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetBrandQuery(int Id, bool TrackChanges) : IRequest<BrandDto>;
}
