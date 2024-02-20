using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record CreateItemCommand(
        CreateItemDTO ItemDTO,
        Guid BrandId,
        Guid TypeId,
        Guid VendorId
        ) : IRequest<ItemDTO>;
}
