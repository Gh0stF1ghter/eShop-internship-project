namespace Baskets.API.Producer
{
    public interface IMessageProducer
    {
        void Send<T>(T message);
    }
}
