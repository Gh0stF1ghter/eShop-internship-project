using Baskets.BusinessLogic.CQRS.Commands.BasketItemCommands.UpdateBasketItem;
using Baskets.BusinessLogic.CQRS.Queries.BasketItemQueries.GetBasketItem;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Baskets.API.Hubs
{
    public class BasketHub(ISender sender) : Hub
    {
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