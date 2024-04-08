using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.VendorQueries.GetVendor
{
    public sealed record GetVendorQuery(int Id, bool TrackChanges) : IRequest<VendorDto>;
}
