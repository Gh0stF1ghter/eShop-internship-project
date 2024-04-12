using Catalogs.Application.Comands.ItemTypeCommands;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class DeleteItemTypeHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemTypeComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemTypeComand comand, CancellationToken token)
        {
            var brand = await _unitOfWork.ItemType.GetItemTypeByIdAsync(comand.Id, comand.TrackChanges, token);

            if (brand == null)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            _unitOfWork.ItemType.Delete(brand);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
