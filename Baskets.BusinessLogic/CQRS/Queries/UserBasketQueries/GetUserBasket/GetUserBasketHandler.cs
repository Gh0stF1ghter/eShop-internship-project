using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasket
{
    public class GetUserBasketHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetUserBasketQuery, UserBasketDto>
    {
        public async Task<UserBasketDto> Handle(GetUserBasketQuery query, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(query.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }

            var basketDto = mapper.Map<UserBasketDto>(basket);

            return basketDto;
        }
    }
}