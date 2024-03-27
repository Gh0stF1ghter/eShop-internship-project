using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.VendorQueries
{
    public sealed record GetVendorsQuery(bool TrackChanges) : IRequest<IEnumerable<VendorDto>>;
}
