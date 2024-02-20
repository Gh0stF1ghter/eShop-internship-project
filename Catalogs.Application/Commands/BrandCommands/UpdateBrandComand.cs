using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record UpdateBrandCommand(int Id, BrandManipulateDto Brand, bool TrackChanges) : IRequest;
}
