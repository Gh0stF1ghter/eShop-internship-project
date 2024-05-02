using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.BrandCommands.UpdateBrand
{
    public sealed record UpdateBrandCommand(int Id, BrandManipulateDto Brand, bool TrackChanges) : IRequest;
}
