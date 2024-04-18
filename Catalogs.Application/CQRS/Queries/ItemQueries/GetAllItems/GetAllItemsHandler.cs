using Catalogs.Application.DataTransferObjects;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using MediatR;

namespace Catalogs.Application.CQRS.Queries.ItemQueries.GetAllItems
{
    public class GetAllItemsHandler(IUnitOfWork unitOfWork, IItemLinks<ItemDto> itemLinks, IMapper mapper) : IRequestHandler<GetAllItemsQuery, (IEnumerable<ItemDto> items, MetaData metaData)>
    {
        public async Task<(IEnumerable<ItemDto> items, MetaData metaData)> Handle(GetAllItemsQuery query, CancellationToken cancellationToken)
        {
            var items = await unitOfWork.Item.GetAllItemsAsync(query.ItemParameters, cancellationToken);

            var itemDtos = mapper.Map<IEnumerable<ItemDto>>(items);

            return (items: itemDtos, metaData: items.MetaData);
        }
    }
}
