using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record CreateItemComand(ItemManipulateDto ItemDTO, int TypeId, bool TrackChanges) : IRequest<ItemDto>;
}
