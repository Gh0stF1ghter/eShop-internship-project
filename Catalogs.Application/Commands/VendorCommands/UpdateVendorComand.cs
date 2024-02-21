﻿using Catalogs.Domain.Entities.DataTransferObjects.CreateDTOs;
using MediatR;

namespace Catalogs.Application.Commands.ItemCommands
{
    public record UpdateVendorComand(int Id, VendorManipulateDto Vendor, bool TrackChanges) : IRequest;
}
