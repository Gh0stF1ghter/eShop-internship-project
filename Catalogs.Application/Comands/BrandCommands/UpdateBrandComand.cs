using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.BrandCommands
{
    public sealed record UpdateBrandComand(int Id, BrandManipulateDto Brand, bool TrackChanges) : IRequest;
}
