using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemTypeCommands.UpdateItemType
{
    public sealed record UpdateItemTypeCommand(int Id, ItemTypeManipulateDto Type, bool TrackChanges) : IRequest;
}
