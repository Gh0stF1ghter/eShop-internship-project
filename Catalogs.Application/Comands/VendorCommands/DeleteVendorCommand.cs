using MediatR;

namespace Catalogs.Application.Comands.VendorCommands
{
    public sealed record DeleteVendorComand(int Id, bool TrackChanges) : IRequest;
}
