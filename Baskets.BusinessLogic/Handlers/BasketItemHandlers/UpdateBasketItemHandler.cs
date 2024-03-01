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
            var item = await unitOfWork.Item.GetByConditionAsync(i => i.Id.Equals(comand.ItemId), cancellationToken);

            await FindReferences(comand, item, cancellationToken);

            var itemToUpdate = await unitOfWork.BasketItem.GetByConditionAsync(bi => bi.ItemId.Equals(comand.ItemId) && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (itemToUpdate == null)
            {
                throw new BadRequestException(BasketItemMessages.NotInCurrentBasket);
            }

            itemToUpdate.Quantity = comand.Quantity;
            itemToUpdate.SumPrice = item.Price * comand.Quantity;

            unitOfWork.BasketItem.Update(bi => bi.ItemId.Equals(comand.ItemId), itemToUpdate);
        }

        private async Task FindReferences(UpdateBasketItemComand comand, Item item, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.User.GetByConditionAsync(u => u.Id.Equals(comand.UserId), cancellationToken);

            if (user == null)
            {
                throw new BadRequestException(UserMessages.NotFound);
            }

            if (item == null)
            {
                throw new BadRequestException(ItemMessages.NotFound);
            }
        }

    }
}
