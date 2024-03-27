using Catalogs.Application.Comands.ItemTypeCommands;
using Catalogs.Domain.Entities.Models;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class UpdateItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateItemTypeComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateItemTypeComand request, CancellationToken token)
        {
            var itemTypeToUpdate = await _unitOfWork.ItemType.GetItemTypeByIdAsync(request.Id, request.TrackChanges, token);

            if (itemTypeToUpdate == null)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            _mapper.Map(request.Type, itemTypeToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
