using Catalogs.Application.Queries.ItemQueries;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class GetItemTypesHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemTypesQuery, IEnumerable<ItemTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemTypeDto>> Handle(GetItemTypesQuery query, CancellationToken token)
        {
            var types = await _unitOfWork.ItemType.GetAllItemTypesAsync(query.TrackChanges, token);

            var typeDtos = _mapper.Map<IEnumerable<ItemTypeDto>>(types);

            return typeDtos;
        }
    }
}
