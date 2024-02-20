using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemDto>> Handle(GetItemsQuery query, CancellationToken token)
        {
            var items = await _unitOfWork.Item.GetAllItemsAsync(query.TrackChanges);

            var itemsDto = _mapper.Map<IEnumerable<ItemDto>>(items);

            return itemsDto;
        }
    }
}
