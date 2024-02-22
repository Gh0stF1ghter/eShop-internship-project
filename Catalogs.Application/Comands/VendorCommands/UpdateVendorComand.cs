using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record UpdateVendorComand(int Id, VendorManipulateDto Vendor, bool TrackChanges) : IRequest;
}
