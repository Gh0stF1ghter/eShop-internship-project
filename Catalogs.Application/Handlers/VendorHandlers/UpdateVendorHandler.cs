using Catalogs.Application.Comands.VendorCommands;
using MediatR;

namespace Catalogs.Application.Handlers.VendorHandlers
{
    internal class UpdateVendorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateVendorComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateVendorComand request, CancellationToken token)
        {
            var vendorToUpdate = await _unitOfWork.Vendor.GetVendorByIdAsync(request.Id, request.TrackChanges, token);

            if (vendorToUpdate == null)
            {
                throw new BadRequestException(VendorMessages.VendorNotFound);
            }

            _mapper.Map(request.Vendor, vendorToUpdate);
            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
