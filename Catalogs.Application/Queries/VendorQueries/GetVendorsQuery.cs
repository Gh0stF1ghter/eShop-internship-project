using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetVendorsQuery(bool TrackChanges) : IRequest<IEnumerable<VendorDto>>;
}
