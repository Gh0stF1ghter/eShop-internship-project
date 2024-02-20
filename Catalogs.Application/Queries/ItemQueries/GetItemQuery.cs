﻿using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetItemQuery(int Id, bool TrackChanges) : IRequest<ItemDto>;
}
