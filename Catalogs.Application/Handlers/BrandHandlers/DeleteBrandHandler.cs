using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class DeleteBrandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBrandComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteBrandComand comand, CancellationToken token)
        {
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(comand.Id, comand.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.BrandNotFound);

            _unitOfWork.Brand.DeleteBrand(brand);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
