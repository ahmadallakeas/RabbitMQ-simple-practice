namespace Rabbit.API.Interfaces
{
    public interface IMessageProducer
    {
        public void SendMessage<T>(T message);
    }
}
