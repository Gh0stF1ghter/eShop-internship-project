using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public sealed record CreateBrandComand(BrandManipulateDto BrandDto) : IRequest<BrandDto>;
}
