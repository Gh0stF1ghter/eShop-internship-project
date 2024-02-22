using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.RequestFeatures;
using MediatR;
using System.Dynamic;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public class GetItemsOfTypeHandler(IUnitOfWork unitOfWork, IMapper mapper, IDataShaper<ItemDto> dataShaper) : IRequestHandler<GetItemsOfTypeQuery, (IEnumerable<ExpandoObject> items, MetaData metaData)>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IDataShaper<ItemDto> _dataShaper = dataShaper;

        public async Task<(IEnumerable<ExpandoObject> items, MetaData metaData)> Handle(GetItemsOfTypeQuery query, CancellationToken token)
        {
            var itemType = await _unitOfWork.ItemType.GetItemTypeByIdAsync(query.TypeId, query.TrackChanges, token)
                ?? throw new NotFoundException(ErrorMessages.TypeNotFound);

            var items = await _unitOfWork.Item.GetAllItemsOfTypeAsync(query.TypeId, query.ItemParameters, query.TrackChanges, token);

            var itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            var shapedData = _dataShaper.ShapeData(itemDtos, query.ItemParameters.Fields);

            return (items: shapedData, metaData: items.MetaData);
        }
    }
}
