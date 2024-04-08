using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemTypeCommands.CreateItemType
{
    public sealed record CreateItemTypeCommand(ItemTypeManipulateDto ItemTypeDto) : IRequest<ItemTypeDto>;
}
