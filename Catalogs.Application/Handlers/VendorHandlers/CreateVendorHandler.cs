using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Comands.VendorCommands;
using MediatR;

namespace Catalogs.Application.Handlers.VendorHandlers
{
    public sealed class CreateVendorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateVendorCommand, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<VendorDto> Handle(CreateVendorCommand command, CancellationToken token)
        {
            if (command.VendorDto is null)
            {
                throw new BadRequestException(VendorMessages.VendorIsNull);
            }

            var vendor = _mapper.Map<Vendor>(command.VendorDto);

            _unitOfWork.Vendor.Add(vendor);
            await _unitOfWork.SaveChangesAsync(token);

            var vendorToReturn = _mapper.Map<VendorDto>(vendor);

            return vendorToReturn;
        }
    }
}
