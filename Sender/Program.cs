using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare("workers", durable: true, exclusive: false, autoDelete: false, arguments: null);
    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);
    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;
    channel.BasicPublish("", routingKey: "workers", properties, body);
    Console.WriteLine(" [x] Sent {0}", message);
}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();



string GetMessage(string[] args)
{
    return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
}
