using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record UpdateBrandComand(int Id, BrandManipulateDto Brand, bool TrackChanges) : IRequest;
}
