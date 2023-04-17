using RabbitMQ.Client;

namespace FormulaAirLine.API.Services;

public static class RabbitMQConfiguration
{
    public static IModel GetChannel(string queue)
    {
        try
        {
            IConnection connection = CreateConnectionFactory();
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queue: queue,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
            return channel;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception: {ex.GetType().FullName} | Message: {ex.Message}", ex);
        }
    }

    private static IConnection CreateConnectionFactory()
    {
        ConnectionFactory factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "mypass",
            VirtualHost = "/"
        };

        return factory.CreateConnection();
    }
}