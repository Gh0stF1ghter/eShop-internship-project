using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.RequestFeatures;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public class GetItemsOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemsOfTypeQuery, (IEnumerable<ItemDto> items, MetaData metaData)>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<(IEnumerable<ItemDto> items, MetaData metaData)> Handle(GetItemsOfTypeQuery query, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(query.TypeId, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);

            var items = await _unitOfWork.Item.GetAllItemsOfTypeAsync(query.TypeId, query.ItemParameters, query.TrackChanges, token);

            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            return (items: itemDtos, metaData: items.MetaData);
        }
    }
}
