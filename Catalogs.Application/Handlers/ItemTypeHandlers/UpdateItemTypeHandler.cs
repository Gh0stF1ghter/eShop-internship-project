using Catalogs.Application.Comands.ItemTypeCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class UpdateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemTypeCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemTypeCommand request, CancellationToken token)
        {
            var itemTypeToUpdate = await _unitOfWork.ItemType.GetItemTypeByIdAsync(request.Id, request.TrackChanges, token)
                ?? throw new BadRequestException(ItemTypeMessages.TypeNotFound);

            _mapper.Map(request.Type, itemTypeToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
