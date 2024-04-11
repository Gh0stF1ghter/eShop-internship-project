using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemTypeCommands.UpdateItemType
{
    public sealed class UpdateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemTypeCommand request, CancellationToken token)
        {
            var itemTypeToUpdate = await _unitOfWork.ItemType.GetItemTypeToUpdateAsync(request.Id, token);

            if (itemTypeToUpdate == null)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            _mapper.Map(request.Type, itemTypeToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
