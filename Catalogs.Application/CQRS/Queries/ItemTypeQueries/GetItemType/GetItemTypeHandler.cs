using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemTypeQueries.GetItemType
{
    public sealed class GetItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemTypeQuery, ItemTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemTypeDto> Handle(GetItemTypeQuery query, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(query.Id, query.TrackChanges, token);

            if (itemType == null)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            var itemTypeDto = _mapper.Map<ItemTypeDto>(itemType);

            return itemTypeDto;
        }
    }
}
