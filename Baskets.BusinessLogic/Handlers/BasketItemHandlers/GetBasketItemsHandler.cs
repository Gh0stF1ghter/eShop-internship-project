using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.BusinessLogic.Queries.BasketItem;
using Baskets.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class GetBasketItemsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBasketItemsQuery, IEnumerable<BasketItemDto>>
    {
        public Task<IEnumerable<BasketItemDto>> Handle(GetBasketItemsQuery query, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
