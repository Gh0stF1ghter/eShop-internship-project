using Catalogs.Application.Commands.ItemCommands;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class DeleteItemHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteItemCommand command, CancellationToken token)
        {
            var item = await _unitOfWork.Item.GetItemByIdAsync(command.Id, command.TrackChanges)
                ?? throw new BadRequestException(ErrorMessages.ItemNotFound);

            _unitOfWork.Item.Delete(item);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
