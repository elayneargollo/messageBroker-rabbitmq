using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace FormulaAirLine.API.Services;

public class MessageProducer : IMessageProducer
{
    private const string QUEUE = "bookings";

    public void SendingMessage<T>(T message)
    {
        try
        {
            using IModel channel = RabbitMQConfiguration.GetChannel(queue: QUEUE);
            
            string jsonString = JsonSerializer.Serialize(message);
            byte[] body = Encoding.UTF8.GetBytes(jsonString);

            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: string.Empty,
                                routingKey: QUEUE,
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