using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.VendorCommands
{
    public sealed record CreateVendorCommand(VendorManipulateDto VendorDto) : IRequest<VendorDto>;
}
