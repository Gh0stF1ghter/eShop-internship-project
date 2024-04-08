using Catalogs.Application.DataTransferObjects;
using Catalogs.Application.Queries.ItemQueries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogs.Application.Handlers.ItemHandlers
{
    public sealed class GetItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetItemQuery, ItemDto>
    {
        public async Task<ItemDto> Handle(GetItemQuery query, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.Item.GetItemByIdAsync(query.Id, cancellationToken);

            if (item == null)
            {
                throw new NotFoundException(ItemMessages.ItemNotFound);
            }

            var itemDto = mapper.Map<ItemDto>(item);

            return itemDto;
        }
    }
}
