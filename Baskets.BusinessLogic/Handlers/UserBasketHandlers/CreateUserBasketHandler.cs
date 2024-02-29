﻿using AutoMapper;
using Baskets.BusinessLogic.Comands.CustomerBasket;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.Handlers.CustomerBasketHandlers
{
    public class CreateUserBasketHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserBasketComand, UserBasketDto>
    {
        public async Task<UserBasketDto> Handle(CreateUserBasketComand comand, CancellationToken cancellationToken)
        {
            var userExists = await unitOfWork.User.GetByConditionAsync(u => u.UserId.Equals(comand.UserId), cancellationToken);

            if (userExists == null)
            {
                throw new BadRequestException(UserMessages.NotFound);
            }

            var basketExists = await unitOfWork.Basket.GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basketExists != null)
            {
                throw new BadRequestException(UserBasketMessages.Exists);
            }

            var newBasket = new UserBasket
            {
                UserId = comand.UserId
            };

            unitOfWork.Basket.Add(newBasket);

            var newBasketDto = mapper.Map<UserBasketDto>(newBasket);

            return newBasketDto;
        }
    }
}