using Catalogs.Application.Comands.VendorCommands;
using Catalogs.Domain.Entities.Constants.Messages;
using MediatR;

namespace Catalogs.Application.Handlers.VendorHandlers
{
    public sealed class DeleteVendorHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteVendorComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteVendorComand comand, CancellationToken token)
        {
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(comand.Id, comand.TrackChanges, token);

            if (vendor == null)
            {
                throw new NotFoundException(VendorMessages.VendorNotFound);
            }

            _unitOfWork.Vendor.Delete(vendor);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
