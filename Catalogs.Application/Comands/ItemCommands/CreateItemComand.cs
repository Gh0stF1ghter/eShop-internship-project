using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.ItemCommands
{
    public sealed record CreateItemComand(ItemManipulateDto ItemDTO, int TypeId, bool TrackChanges) : IRequest<ItemDto>;
}
