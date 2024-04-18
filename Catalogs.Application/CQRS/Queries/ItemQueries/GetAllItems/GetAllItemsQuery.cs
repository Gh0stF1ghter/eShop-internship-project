using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Application.CQRS.Queries.ItemQueries.GetAllItems
{
    public record GetAllItemsQuery(ItemParameters ItemParameters) : IRequest<(IEnumerable<ItemDto> items, MetaData metaData)>;
}
