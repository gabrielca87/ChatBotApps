using RabbitMQ.Client;
using System.Text;

namespace Chat.Server.Services.QueueService
{
    public class QueuePublisherService : IQueuePublisherService
    {
        const string HOST_NAME = "localhost";
        const string QUEUE_STOCK = "queue_stock";

        public void Publish(string message)
        {
            var factory = new ConnectionFactory() { HostName = HOST_NAME };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QUEUE_STOCK, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty, routingKey: QUEUE_STOCK, basicProperties: null, body: body);
            }
        }
    }
}
