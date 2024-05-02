using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.VendorQueries.GetVendors
{
    public sealed record GetVendorsQuery(bool TrackChanges) : IRequest<IEnumerable<VendorDto>>;
}
