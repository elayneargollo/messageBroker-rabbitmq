using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace FormulaAirLine.API.Services;

public class MessageProducer : IMessageProducer
{
    public void SendingMessage<T>(T message)
    {
        try
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "user",
                Password = "mypass",
                VirtualHost = "/"
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare(queue: "bookings",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            string jsonString = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(jsonString);

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                                routingKey: "bookings",
                                basicProperties: properties,
                                body: body);

            Console.WriteLine($" [x] Sent {message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception: {ex.GetType().FullName} | Message: {ex.Message}", ex);
        }
    }
}