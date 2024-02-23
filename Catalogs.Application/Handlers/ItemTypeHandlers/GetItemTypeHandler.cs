﻿using Catalogs.Application.Queries.ItemQueries;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class GetItemTypeHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemTypeQuery, ItemTypeDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemTypeDto> Handle(GetItemTypeQuery query, CancellationToken token)
        {
            var brand = await _unitOfWork.ItemType.GetItemTypeByIdAsync(query.Id, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);

            var itemType = _mapper.Map<ItemTypeDto>(brand);

            return itemType;
        }
    }
}