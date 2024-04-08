using AutoMapper;
using Baskets.BusinessLogic.Exceptions;
using Baskets.DataAccess.UnitOfWork;

namespace Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem
{
    public class UpdateBasketItemHandler(IUnitOfWork unitOfWork,
        ItemGrpcService.ItemService.ItemServiceClient client,
        IMapper mapper) : IRequestHandler<UpdateBasketItemCommand>
    {
        public async Task Handle(UpdateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var basket = await FindBasket(comand, cancellationToken);

            var basketItemToUpdate = await FindInBasket(comand, cancellationToken);
            basketItemToUpdate.Item = await FindCatalogItemAsync(basketItemToUpdate.ItemId, cancellationToken);

            basket.TotalPrice -= basketItemToUpdate.SumPrice;

            basketItemToUpdate.Quantity = comand.Quantity;
            basketItemToUpdate.SumPrice = basketItemToUpdate.Item.Price * comand.Quantity;

            await unitOfWork.BasketItem
                .UpdateAsync(bi => bi.BasketItemId.Equals(comand.BasketItemId), basketItemToUpdate, cancellationToken);

            await UpdateTotalCost(basket, basketItemToUpdate, cancellationToken);
        }

        private async Task<UserBasket> FindBasket(UpdateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var basket = await unitOfWork.Basket
                .GetByConditionAsync(b => b.UserId.Equals(comand.UserId), cancellationToken);

            if (basket == null)
            {
                throw new NotFoundException(UserBasketMessages.NotFound);
            }

            return basket;
        }

        private async Task<BasketItem> FindInBasket(UpdateBasketItemCommand comand, CancellationToken cancellationToken)
        {
            var itemInBasket = await unitOfWork.BasketItem
                .GetBasketItemByConditionAsync(bi => bi.BasketItemId.Equals(comand.BasketItemId)
                && bi.UserId.Equals(comand.UserId), cancellationToken);

            if (itemInBasket == null)
            {
                throw new NotFoundException(BasketItemMessages.NotFound);
            }

            return itemInBasket;
        }

        private async Task UpdateTotalCost(UserBasket basket, BasketItem newItem, CancellationToken cancellationToken)
        {
            basket.TotalPrice += newItem.SumPrice;

            await unitOfWork.Basket
                .UpdateAsync(u => u.UserId.Equals(basket.UserId), basket, cancellationToken);
        }

        private async Task<Item> FindCatalogItemAsync(int itemId, CancellationToken cancellationToken = default)
        {
            var itemRequest = new ItemGrpcService.GetItemRequest { Id = itemId };

            var itemResponse = await client.GetItemAsync(itemRequest, cancellationToken: cancellationToken);

            var grpcItem = itemResponse.Item;

            if (grpcItem == null)
            {
                throw new NotFoundException(ItemMessages.NotFound);
            }

            var item = mapper.Map<Item>(grpcItem);

            return item;
        }
    }
}