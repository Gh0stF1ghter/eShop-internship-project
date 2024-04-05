using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket;
using MassTransit;
using MediatR;
using RabbitMQ.EventBus;

namespace Baskets.API.Consumers
{
    public class UserCreatedConsumer(ISender sender) : IConsumer<UserCreated>
    {
        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            string userId = context.Message.UserId;

            await Console.Out.WriteLineAsync(userId + " consumed");

            await sender.Send(new CreateUserBasketCommand(userId));
        }
    }
}