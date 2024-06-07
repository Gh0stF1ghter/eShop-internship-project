using MediatR;

namespace Catalogs.Application.CQRS.Commands.VendorCommands.DeleteVendor
{
    public sealed record DeleteVendorCommand(int Id, bool TrackChanges) : IRequest;
}
