using Rabbit.API.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Rabbit.API.Services
{
    public class MessageProducer : IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "mrreaper72",
                Password = "Ahm@d72lakeas",
                VirtualHost = "/"
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("Reservations", durable: true, exclusive: false);
            var jsonString = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(jsonString);

            channel.BasicPublish("", "Reservations", body: body);
        }
    }
}
