using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetVendorQuery(int Id, bool TrackChanges) : IRequest<VendorDto>;
}
