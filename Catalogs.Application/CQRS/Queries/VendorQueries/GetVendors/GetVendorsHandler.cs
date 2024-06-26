﻿using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.VendorQueries.GetVendors
{
    public sealed class GetVendorsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetVendorsQuery, IEnumerable<VendorDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<VendorDto>> Handle(GetVendorsQuery query, CancellationToken token)
        {
            var vendors = await _unitOfWork.Vendor.GetAllVendorsAsync(query.TrackChanges, token);

            var vendorDtos = _mapper.Map<IEnumerable<VendorDto>>(vendors);

            return vendorDtos;
        }
    }
}
