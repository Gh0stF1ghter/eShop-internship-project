using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.VendorCommands
{
    public sealed record UpdateVendorComand(int Id, VendorManipulateDto Vendor, bool TrackChanges) : IRequest;
}
