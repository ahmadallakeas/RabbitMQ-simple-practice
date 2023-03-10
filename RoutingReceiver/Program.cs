using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

    var queueName = channel.QueueDeclare().QueueName;

    if (args.Length < 1)
    {
        Console.Error.WriteLine("Usage: {0} [info] [warning] [error]",
                                Environment.GetCommandLineArgs()[0]);
        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
        Environment.ExitCode = 1;
        return;
    }
    foreach (var severity in args)
    {
        channel.QueueBind(queue: queueName,
                          exchange: "direct_logs",
                          routingKey: severity);
    }

    Console.WriteLine(" [*] Waiting for logs.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, args) =>
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var routingKey = args.RoutingKey;
        Console.WriteLine(" [x] Received '{0}':'{1}'",
                                 routingKey, message);
    };
    channel.BasicConsume(queue: queueName,
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}