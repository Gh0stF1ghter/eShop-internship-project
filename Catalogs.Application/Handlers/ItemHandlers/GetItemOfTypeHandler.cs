using Catalogs.Application.Queries.ItemQueries;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemOfTypeQuery, ItemDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDto> Handle(GetItemOfTypeQuery query, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(query.TypeId, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);

            var item = await _unitOfWork.Item.GetItemOfTypeByIdAsync(query.TypeId, query.Id, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.ItemNotFound + query.Id);

            var itemDto = _mapper.Map<ItemDto>(item);

            return itemDto;
        }
    }
}
