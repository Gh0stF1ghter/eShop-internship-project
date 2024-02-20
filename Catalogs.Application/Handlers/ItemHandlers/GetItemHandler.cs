using AutoMapper;
using Catalogs.Application.Queries.ItemQueries;
using Catalogs.Domain.Entities.Constants;
using Catalogs.Domain.Entities.DataTransferObjects;
using Catalogs.Domain.Entities.Exceptions;
using Catalogs.Domain.Interfaces;
using MediatR;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemQuery, ItemDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ItemDTO> Handle(GetItemQuery query, CancellationToken token)
        {
            var item = await _unitOfWork.Items.GetItemByIdAsync(query.Id, query.TrackChanges)
                ?? throw new NotFoundException(ErrorMessages.ItemNotFound + query.Id);

            var itemDto = _mapper.Map<ItemDTO>(item);

            return itemDto;
        }
    }
}
