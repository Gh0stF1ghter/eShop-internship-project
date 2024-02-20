using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemsQuery, IEnumerable<ItemDTO>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<ItemDTO>> Handle(GetItemsQuery query, CancellationToken token)
        {
            var items = await _unitOfWork.Items.GetAllItemsAsync(query.TrackChanges);
            
            var itemsDto = _mapper.Map<IEnumerable<ItemDTO>>(items);

            return itemsDto;
        }
    }
}
