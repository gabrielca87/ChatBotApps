using Chat.Server.Config;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Chat.Server.Services.QueueService
{
    public class QueuePublisherService : IQueuePublisherService
    {
        private readonly IOptions<RabbitMQSettings> _config;

        public QueuePublisherService(IOptions<RabbitMQSettings> config)
        {
            _config = config;
        }

        public void Publish(string message)
        {
            var factory = new ConnectionFactory() { HostName = _config.Value.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _config.Value.QueueStock, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty, routingKey: _config.Value.QueueStock, basicProperties: null, body: body);
            }
        }
    }
}
