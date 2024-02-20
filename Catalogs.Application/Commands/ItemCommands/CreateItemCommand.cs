using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record CreateItemCommand(
        ItemManipulateDto ItemDTO,
        int BrandId,
        int TypeId,
        int VendorId
        ) : IRequest<ItemDto>;
}
