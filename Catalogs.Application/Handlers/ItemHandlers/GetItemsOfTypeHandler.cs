using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public class GetItemsOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemsOfTypeQuery, IEnumerable<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemDto>> Handle(GetItemsOfTypeQuery query, CancellationToken token)
        {
            var items = await _unitOfWork.Item.GetAllItemsOfTypeAsync(query.TypeId, query.TrackChanges);

            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            return itemDtos;
        }
    }
}
