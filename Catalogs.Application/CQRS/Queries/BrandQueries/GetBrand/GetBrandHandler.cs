﻿using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.BrandQueries.GetBrand
{
    public sealed class GetBrandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBrandQuery, BrandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BrandDto> Handle(GetBrandQuery query, CancellationToken token)
        {
            var brand = await _unitOfWork.Brand.GetBrandByIdAsync(query.Id, query.TrackChanges, token);

            if (brand == null)
            {
                throw new NotFoundException(BrandMessages.BrandNotFound);
            }

            var brandDto = _mapper.Map<BrandDto>(brand);

            return brandDto;
        }
    }
}