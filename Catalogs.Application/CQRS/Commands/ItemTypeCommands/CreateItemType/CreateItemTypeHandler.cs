using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.ItemTypeCommands.CreateItemType
{
    public sealed class CreateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemTypeCommand, ItemTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemTypeDto> Handle(CreateItemTypeCommand command, CancellationToken token)
        {
            var itemmTypeExists = await _unitOfWork.ItemType.IsExistAsync(it => it.Name.Equals(command.ItemTypeDto.Name), token);

            if (itemmTypeExists)
            {
                throw new BadRequestException(ItemTypeMessages.ItemTypeExists);
            }

            var itemType = _mapper.Map<ItemType>(command.ItemTypeDto);

            _unitOfWork.ItemType.Add(itemType);
            await _unitOfWork.SaveChangesAsync(token);

            var itemTypeToReturn = _mapper.Map<ItemTypeDto>(itemType);

            return itemTypeToReturn;
        }
    }
}
