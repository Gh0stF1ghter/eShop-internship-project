using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record UpdateItemCommand(int TypeId, int Id, ItemManipulateDto Item, bool TrackChanges) : IRequest;
}
