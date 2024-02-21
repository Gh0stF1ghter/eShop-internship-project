﻿using Catalogs.Application.Queries.ItemQueries;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class GetVendorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetVendorQuery, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<VendorDto> Handle(GetVendorQuery query, CancellationToken token)
        {
            var brand = await _unitOfWork.Vendor.GetVendorByIdAsync(query.Id, query.TrackChanges)
                ?? throw new NotFoundException(ErrorMessages.VendorNotFound + query.Id);

            var brandDto = _mapper.Map<VendorDto>(brand);

            return brandDto;
        }
    }
}
