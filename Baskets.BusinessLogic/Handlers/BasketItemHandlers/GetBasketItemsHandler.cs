using AutoMapper;
using Baskets.BusinessLogic.Queries.BasketItem;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class GetBasketItemsHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBasketItemsQuery, IEnumerable<BasketItemDto>>
    {
        public async Task<IEnumerable<BasketItemDto>> Handle(GetBasketItemsQuery query, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User.GetByConditionAsync(u => u.Id.Equals(query.UserId), cancellationToken);

            if (user == null)
            {
                throw new BadRequestException(UserMessages.NotFound);
            }

            var items = await unitOfWork.BasketItem.GetAllAsync(cancellationToken);

            var itemDtos = mapper.Map<IEnumerable<BasketItemDto>>(items);

            return itemDtos;
        }
    }
}
