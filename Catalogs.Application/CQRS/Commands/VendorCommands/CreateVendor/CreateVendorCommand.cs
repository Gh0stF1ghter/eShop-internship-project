using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.VendorCommands.CreateVendor
{
    public sealed record CreateVendorCommand(VendorManipulateDto VendorDto) : IRequest<VendorDto>;
}
