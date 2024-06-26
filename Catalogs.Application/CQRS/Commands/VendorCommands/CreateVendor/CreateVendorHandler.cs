using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.VendorCommands.CreateVendor
{
    public sealed class CreateVendorHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateVendorCommand, VendorDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<VendorDto> Handle(CreateVendorCommand command, CancellationToken token)
        {
            var vendorExists = await _unitOfWork.Vendor.IsExistAsync(v => v.Name.Equals(command.VendorDto.Name), token);

            if (vendorExists)
            {
                throw new BadRequestException(VendorMessages.VendorExists);
            }

            var vendor = _mapper.Map<Vendor>(command.VendorDto);

            _unitOfWork.Vendor.Add(vendor);
            await _unitOfWork.SaveChangesAsync(token);

            var vendorToReturn = _mapper.Map<VendorDto>(vendor);

            return vendorToReturn;
        }
    }
}
