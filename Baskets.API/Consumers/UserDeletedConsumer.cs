using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket;
using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.DeleteUserBasket;
using MassTransit;
using MediatR;
using RabbitMQ.EventBus;

namespace Baskets.API.Consumers
{
    public class UserDeletedConsumer(ISender sender) : IConsumer<UserDeleted>
    {
        public async Task Consume(ConsumeContext<UserDeleted> context)
        {
            string userId = context.Message.UserId;

            await Console.Out.WriteLineAsync(userId + " consumed");

            await sender.Send(new DeleteUserBasketCommand(userId));
        }
    }
}