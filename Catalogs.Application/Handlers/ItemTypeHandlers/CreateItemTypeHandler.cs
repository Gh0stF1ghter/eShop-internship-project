using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class CreateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateItemTypeCommand, ItemTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemTypeDto> Handle(CreateItemTypeCommand command, CancellationToken token)
        {
            if (command.ItemTypeDto is null)
            {
                throw new BadRequestException(ErrorMessages.ItemTypeIsNull);
            }

            var itemType = _mapper.Map<ItemType>(command.ItemTypeDto);

            _unitOfWork.ItemType.Add(itemType);
            await _unitOfWork.SaveChangesAsync(token);

            var itemTypeToReturn = _mapper.Map<ItemTypeDto>(itemType);

            return itemTypeToReturn;
        }
    }
}
