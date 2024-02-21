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
            if (command.TypeDto is null)
            {
                throw new BadRequestException(ErrorMessages.TypeIsNull);
            }

            var type = _mapper.Map<ItemType>(command.TypeDto);

            _unitOfWork.ItemType.Add(type);
            await _unitOfWork.SaveChangesAsync(token);

            var typeToReturn = _mapper.Map<ItemTypeDto>(type);

            return typeToReturn;
        }
    }
}
