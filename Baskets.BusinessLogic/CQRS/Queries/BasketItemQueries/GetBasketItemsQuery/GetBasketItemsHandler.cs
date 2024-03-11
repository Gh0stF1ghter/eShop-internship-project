using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItemsQuery
{
    public class GetBasketItemsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBasketItemsQuery, IEnumerable<BasketItemDto>>
    {
        public async Task<IEnumerable<BasketItemDto>> Handle(GetBasketItemsQuery query, CancellationToken cancellationToken)
        {
            await FindBasketAsync(query, cancellationToken);

            var items = await unitOfWork.BasketItem
                .GetAllBasketItemsAsync(query.UserId, cancellationToken);

            var itemDtos = mapper.Map<IEnumerable<BasketItemDto>>(items);

            return itemDtos;
        }

        private async Task FindBasketAsync(GetBasketItemsQuery query, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(query.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }
        }
    }
}