using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Comands.BrandCommands
{
    public sealed record CreateBrandComand(BrandManipulateDto BrandDto) : IRequest<BrandDto>;
}
