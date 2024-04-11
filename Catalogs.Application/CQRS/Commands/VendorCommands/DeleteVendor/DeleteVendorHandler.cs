using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.CQRS.Commands.VendorCommands.DeleteVendor
{
    public sealed class DeleteVendorHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteVendorCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteVendorCommand comand, CancellationToken token)
        {
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(comand.Id, comand.TrackChanges, token);

            if (vendor == null)
            {
                throw new NotFoundException(VendorMessages.VendorNotFound);
            }

            await _unitOfWork.Vendor.DeleteVendorAsync(vendor, token);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
