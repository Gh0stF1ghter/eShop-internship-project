using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record CreateVendorCommand(VendorManipulateDto VendorDto) : IRequest<VendorDto>;
}
