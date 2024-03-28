using Baskets.BusinessLogic.CQRS.Commands.UserBasketCommands.CreateUserBasket;
using Baskets.BusinessLogic.DataTransferObjects;
using MassTransit;
using MediatR;
using RabbitMQ.EventBus;
using System.Text.Json;

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