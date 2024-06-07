using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Baskets.API.Producer
{
    public class MessageProducer : IMessageProducer
    {
        public void Send<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            var connection = factory.CreateConnection();

            using var channel = connection.CreateModel();
            channel.QueueDeclare("users");

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "users", body: body);
        }
    }
}