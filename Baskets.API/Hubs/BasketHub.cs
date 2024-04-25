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

        public async Task CreateBasketItemAsync(string userId, string basketItemId, CancellationToken cancellationToken)
        {
            var basketItem = await sender.Send(new GetBasketItemQuery(userId, basketItemId), cancellationToken);

            await Clients.Caller.SendAsync("BasketItemCreated", basketItem, cancellationToken);
        }

        //replace to user with userId
        public async Task CreateBasketItemNotification()
        {
            await Clients.Caller.SendAsync("CreateBasketItemNotificationReceived");
        }

        //Changing Item quantity will change summary cost
        public async Task UpdateBasketItemQuantity(string userId, string basketItemId, uint quantity, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateBasketItemCommand(userId, basketItemId, (int)quantity), cancellationToken);

            var basketItem = await sender.Send(new GetBasketItemQuery(userId, basketItemId), cancellationToken);

            await Clients.Caller.SendAsync("ItemUpdated", basketItem, cancellationToken);
        }
        //Transfer UpdateBasketItem endpoint functionallity
    }
}