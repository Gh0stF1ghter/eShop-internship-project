using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemOfTypeQuery, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDto> Handle(GetItemOfTypeQuery query, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.IsExistAsync(it => it.Id.Equals(query.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            var item = await _unitOfWork.Item.GetItemOfTypeByIdAsync(query.TypeId, query.Id, query.TrackChanges, token);

            if (item == null)
            {
                throw new NotFoundException(ItemMessages.ItemNotFound);
            }

            var itemDto = _mapper.Map<ItemDto>(item);

            return itemDto;
        }
    }
}