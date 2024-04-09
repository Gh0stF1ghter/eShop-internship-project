using Catalogs.Application.Comands.ItemTypeCommands;
using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class CreateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemTypeComand, ItemTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemTypeDto> Handle(CreateItemTypeComand command, CancellationToken token)
        {
            var itemmTypeExists = await _unitOfWork.ItemType.IsExistAsync(it => it.Name.Equals(command.ItemTypeDto.Name, StringComparison.OrdinalIgnoreCase), token);

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
