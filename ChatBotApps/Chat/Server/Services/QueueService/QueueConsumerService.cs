using Chat.Server.Config;
using Chat.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Services.QueueService
{
    public class QueueConsumerService : IQueueConsumerService
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IOptions<RabbitMQSettings> _config;
        private IConnection _connection;
        private IModel _channel;

        public QueueConsumerService(IHubContext<ChatHub> chatHub, IOptions<RabbitMQSettings> config)
        {
            _chatHub = chatHub;
            _config = config;
        }

        public void Run()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = _config.Value.HostName };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(queue: _config.Value.QueueMessage, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += Consumer_Received;

                _channel.BasicConsume(_config.Value.QueueMessage, autoAck: true, consumer: consumer);
            }
            catch (Exception)
            {
                //TODO: Log
            }
        }

        private void Consumer_Received(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Task.Run(async () =>
            {
                await _chatHub.Clients.All.SendAsync("ReceiveMessage", message, "BOT", DateTime.Now);
            }).Wait();
        }
    }
}
