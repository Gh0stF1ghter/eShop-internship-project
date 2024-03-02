using AutoMapper;
using Baskets.BusinessLogic.Comands.BasketItem;
using Baskets.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baskets.BusinessLogic.Handlers.BasketItemHandlers
{
    public class UpdateBasketItemHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBasketItemComand>
    {
        public async Task Handle(UpdateBasketItemComand comand, CancellationToken cancellationToken)
        {
            await FindReferences(comand, cancellationToken);

            var itemToUpdate = await unitOfWork.BasketItem.GetBasketItemByConditionAsync(bi => bi.Id.Equals(comand.BasketItemId) && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (itemToUpdate == null)
            {
                throw new BadRequestException(BasketItemMessages.NotInCurrentBasket);
            }

            itemToUpdate.Quantity = comand.Quantity;
            itemToUpdate.SumPrice = itemToUpdate.Item.Price * comand.Quantity;

            unitOfWork.BasketItem.Update(bi => bi.Id.Equals(comand.BasketItemId), itemToUpdate);
        }

        private async Task FindReferences(UpdateBasketItemComand comand, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User.GetByConditionAsync(u => u.Id.Equals(comand.UserId), cancellationToken);

            if (user == null)
            {
                throw new BadRequestException(UserMessages.NotFound);
            }
        }

    }
}
