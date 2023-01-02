// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");
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

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"A New reservation is received:\t{message}\n");
};
channel.BasicConsume("Reservations", true, consumer);
Console.ReadKey();