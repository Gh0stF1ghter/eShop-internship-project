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

            await _unitOfWork.Item.DeleteItemAsync(item, token);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
