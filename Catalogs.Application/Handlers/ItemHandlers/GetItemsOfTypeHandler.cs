using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.Constants.Messages;
using Catalogs.Domain.Entities.LinkModels;
using Catalogs.Domain.RequestFeatures;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public class GetItemsOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper, IItemLinks<ItemDto> itemLinks) : IRequestHandler<GetItemsOfTypeQuery, (LinkResponse linkResponse, MetaData metaData)>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IItemLinks<ItemDto> _itemLinks = itemLinks;

        public async Task<(LinkResponse linkResponse, MetaData metaData)> Handle(GetItemsOfTypeQuery query, CancellationToken token)
        {
            var itemTypeExists = await _unitOfWork.ItemType.IsExistAsync(t => t.Id.Equals(query.TypeId), token);

            if (!itemTypeExists)
            {
                throw new NotFoundException(ItemTypeMessages.ItemTypeNotFound);
            }

            var items = await _unitOfWork.Item.GetAllItemsOfTypeAsync(query.TypeId, query.LinkParameters.ItemParameters, query.TrackChanges, token);

            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            var links = _itemLinks.TryGenerateLinks(itemDtos, query.LinkParameters.ItemParameters.Fields, query.TypeId, query.LinkParameters.HttpContext);

            return (linkResponse: links, metaData: items.MetaData);
        }
    }
}
