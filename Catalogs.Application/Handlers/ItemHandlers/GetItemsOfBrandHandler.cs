using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemsOfBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemsOfBrandQuery, IEnumerable<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemDto>> Handle(GetItemsOfBrandQuery query, CancellationToken token)
        {
            var items = await _unitOfWork.Item.GetAllItemsOfBrandAsync(query.BrandId, query.TrackChanges);

            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            return itemDtos;
        }
    }
}
