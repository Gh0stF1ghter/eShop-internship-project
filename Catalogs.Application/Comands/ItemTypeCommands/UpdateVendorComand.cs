using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.ItemTypeCommands
{
    public sealed record UpdateItemTypeCommand(int Id, ItemTypeManipulateDto Type, bool TrackChanges) : IRequest;
}
