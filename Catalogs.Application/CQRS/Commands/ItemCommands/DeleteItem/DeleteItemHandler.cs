using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemCommands.DeleteItem
{
    public sealed class DeleteItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemCommand comand, CancellationToken token)
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
