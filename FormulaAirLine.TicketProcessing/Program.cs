using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine(" [*] Waiting for messages.");

ConnectionFactory factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "mypass",
    VirtualHost = "/"
};

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue:"bookings", 
                     durable: true, 
                     exclusive: false, 
                     autoDelete: false,
                     arguments: null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

consumer.Received += (mode, eventArgs) =>
{
    byte[] body = eventArgs.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);

    Booking? booking = JsonSerializer.Deserialize<Booking>(message);
    
    Console.WriteLine($"From: {booking?.From}");
    Console.WriteLine($"To: {booking?.To}");

    int dots = message.Split('.').Length -1;
    Thread.Sleep(dots * 1000);

    channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);    
};

channel.BasicConsume(queue: "bookings", 
                     autoAck: true, 
                     consumer: consumer);

Console.ReadKey();
Console.WriteLine(" [x] Done");