using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.BrandCommands.CreateBrand
{
    public sealed record CreateBrandCommand(BrandManipulateDto BrandDto) : IRequest<BrandDto>;
}
