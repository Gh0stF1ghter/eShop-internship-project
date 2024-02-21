using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class DeleteItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemCommand comand, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(comand.TypeId, comand.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);

            var item = await _unitOfWork.Item.GetItemOfTypeByIdAsync(comand.TypeId, comand.Id, comand.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.ItemNotFound);

            _unitOfWork.Item.Delete(item);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
