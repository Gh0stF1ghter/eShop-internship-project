﻿using Catalogs.Application.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.VendorQueries
{
    public sealed record GetVendorQuery(int Id, bool TrackChanges) : IRequest<VendorDto>;
}