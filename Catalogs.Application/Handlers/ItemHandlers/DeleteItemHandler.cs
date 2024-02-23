using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class DeleteItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemComand comand, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.Exists(it => it.Id.Equals(comand.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ErrorMessages.TypeNotFound);
            }

            var item = await _unitOfWork.Item.GetItemOfTypeByIdAsync(comand.TypeId, comand.Id, comand.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.ItemNotFound);

            _unitOfWork.Item.Delete(item);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
