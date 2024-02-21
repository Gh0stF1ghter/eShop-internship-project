using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record UpdateItemTypeCommand(int Id, ItemTypeManipulateDto Type, bool TrackChanges) : IRequest;
}
