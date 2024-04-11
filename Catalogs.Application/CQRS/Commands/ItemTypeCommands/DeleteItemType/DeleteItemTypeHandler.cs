using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemTypeCommands.DeleteItemType
{
    public sealed class DeleteItemTypeHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemTypeCommand comand, CancellationToken token)
        {
            var brand = await _unitOfWork.ItemType.GetItemTypeByIdAsync(comand.Id, comand.TrackChanges, token);

            if (brand == null)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            await _unitOfWork.ItemType.DeleteItemTypeAsync(brand, token);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}