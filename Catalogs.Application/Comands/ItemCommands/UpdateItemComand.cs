using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.ItemCommands
{
    public sealed record UpdateItemComand(int TypeId, int Id, ItemManipulateDto Item, bool TrackChanges) : IRequest;
}
