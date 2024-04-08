using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemCommands.CreateItem
{
    public sealed record CreateItemCommand(ItemManipulateDto ItemDTO, int TypeId, bool TrackChanges) : IRequest<ItemDto>;
}
