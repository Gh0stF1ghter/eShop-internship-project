using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.VendorCommands.UpdateVendor
{
    public sealed record UpdateVendorCommand(int Id, VendorManipulateDto Vendor, bool TrackChanges) : IRequest;
}
