﻿using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemTypeQueries.GetItemTypes
{
    public sealed class GetItemTypesHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemTypesQuery, IEnumerable<ItemTypeDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemTypeDto>> Handle(GetItemTypesQuery query, CancellationToken token)
        {
            var itemTypes = await _unitOfWork.ItemType.GetAllItemTypesAsync(query.TrackChanges, token);

            var itemTypesDto = _mapper.Map<IEnumerable<ItemTypeDto>>(itemTypes);

            return itemTypesDto;
        }
    }
}
