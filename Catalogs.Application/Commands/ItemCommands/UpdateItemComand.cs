using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record UpdateItemCommand(int Id, ItemManipulateDto Item, bool TrackChanges) : IRequest;
}
