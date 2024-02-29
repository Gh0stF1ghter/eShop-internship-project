using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.BusinessLogic.Queries.BasketItem;
using Baskets.BusinessLogic.Queries.CustomerBasket;
using Baskets.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class GetUserBasketHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserBasketQuery, UserBasketDto>
    {
        public async Task<UserBasketDto> Handle(GetUserBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket.GetByConditionAsync(b => b.UserId.Equals(query.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }

            var basketDto = mapper.Map<UserBasketDto>(basket);

            return basketDto;
        }
    }
}
