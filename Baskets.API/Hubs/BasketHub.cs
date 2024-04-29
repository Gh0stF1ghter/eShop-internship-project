using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItems;
using Baskets.BusinessLogic.CQRS.Queries.UserBasketQueries.GetUserBasket;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Baskets.API.Hubs
{
    public class BasketHub(ISender sender) : Hub
    {
        public async Task GetUserBasket(string userId)
        {
            var basket = await sender.Send(new GetUserBasketQuery(userId));

            await Clients.Caller.SendAsync("userBasketReceived", basket);
        }

        public async Task GetBasketItems(string userId)
        {
            var basketItems = await sender.Send(new GetBasketItemsQuery(userId));

            await Clients.Caller.SendAsync("basketItemsReceived", basketItems);
        }

        public async Task CreateBasketItem(string userId, string basketItemId, CancellationToken cancellationToken)
        {
            var basketItem = await sender.Send(new GetBasketItemQuery(userId, basketItemId), cancellationToken);

            await Clients.Caller.SendAsync("BasketItemCreated", basketItem, cancellationToken);
        }

        public async Task CreateBasketItemNotification()
        {
            await Clients.Caller.SendAsync("CreateBasketItemNotificationReceived");
        }

        public async Task UpdateBasketItemQuantity(string userId, string basketItemId, uint quantity)
        {
            await sender.Send(new UpdateBasketItemCommand(userId, basketItemId, (int)quantity), cancellationToken: default);

            await GetUserBasket(userId);
            await GetBasketItems(userId);
        }
    }
}