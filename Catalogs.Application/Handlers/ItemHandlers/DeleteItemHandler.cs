using Catalogs.Application.Comands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class DeleteItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemComand comand, CancellationToken token)
        {
            var item = await _unitOfWork.Item.GetItemOfTypeByIdAsync(comand.TypeId, comand.Id, comand.TrackChanges, token);

            if (item == null)
            {
                throw new NotFoundException(ItemMessages.ItemNotFound);
            }

            _unitOfWork.Item.Delete(item);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
