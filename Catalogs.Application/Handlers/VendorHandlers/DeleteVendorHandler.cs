﻿using Catalogs.Application.Commands.ItemCommands;
using MediatR;

namespace Catalogs.Application.Handlers.ItemTypeHandlers
{
    public sealed class DeleteVendorHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteVendorComand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(DeleteVendorComand comand, CancellationToken token)
        {
            var vendor = await _unitOfWork.Vendor.GetVendorByIdAsync(comand.Id, comand.TrackChanges, token)
                ?? throw new BadRequestException(ErrorMessages.VendorNotFound);

            _unitOfWork.Vendor.Delete(vendor);

            await _unitOfWork.SaveChangesAsync(token);
        }
    }
}
