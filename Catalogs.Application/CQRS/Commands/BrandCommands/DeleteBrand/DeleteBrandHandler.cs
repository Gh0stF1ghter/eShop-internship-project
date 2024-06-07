using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.BrandCommands.DeleteBrand
{
    public sealed class DeleteBrandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBrandCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteBrandCommand comand, CancellationToken token)
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
