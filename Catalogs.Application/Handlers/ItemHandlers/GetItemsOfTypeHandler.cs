using Catalogs.Application.Queries.ItemQueries;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public class GetItemsOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemsOfTypeQuery, IEnumerable<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemDto>> Handle(GetItemsOfTypeQuery query, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(query.TypeId, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);

            var items = await _unitOfWork.Item.GetAllItemsOfTypeAsync(query.TypeId, query.TrackChanges, token);

            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            return itemDtos;
        }
    }
}
