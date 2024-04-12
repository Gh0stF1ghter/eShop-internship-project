using Catalogs.Application.Comands.BrandCommands;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.Handlers.BrandHandlers
{
    public sealed class DeleteBrandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBrandComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteBrandComand comand, CancellationToken token)
        {
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(comand.Id, comand.TrackChanges, token);

            if (brand == null)
            {
                throw new NotFoundException(BrandMessages.BrandNotFound);
            }

            _unitOfWork.Brand.Delete(brand);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
