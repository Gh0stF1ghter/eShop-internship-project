using Catalogs.Domain.Entities.DataTransferObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Application.Queries.ItemQueries
{
    public sealed record GetItemQuery(Guid Id, bool TrackChanges) : IRequest<ItemDTO>;
}
