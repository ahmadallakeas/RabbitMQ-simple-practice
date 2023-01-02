using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("workers", durable: true, exclusive: false, autoDelete: false, arguments: null);
    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    Console.WriteLine(" [*] Waiting for messages.");
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, args) =>
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);
        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 1000);

        Console.WriteLine(" [x] Done");

        channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);

    };
    channel.BasicConsume("workers", autoAck: false, consumer: consumer);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}