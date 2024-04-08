using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemCommands.UpdateItem
{
    public sealed record UpdateItemCommand(int TypeId, int Id, ItemManipulateDto Item, bool TrackChanges) : IRequest;
}
