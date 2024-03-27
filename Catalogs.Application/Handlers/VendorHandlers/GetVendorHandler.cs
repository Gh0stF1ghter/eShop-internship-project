using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Queries.VendorQueries;
using Catalogs.Domain.Entities.Models;
using MediatR;

namespace Catalogs.Application.Handlers.VendorHandlers
{
    public sealed class GetVendorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetVendorQuery, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<VendorDto> Handle(GetVendorQuery query, CancellationToken token)
        {
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(query.Id, query.TrackChanges, token);

            if (vendor == null)
            {
                throw new NotFoundException(VendorMessages.VendorNotFound);
            }

            var vendorDto = _mapper.Map<VendorDto>(vendor);

            return vendorDto;
        }
    }
}
