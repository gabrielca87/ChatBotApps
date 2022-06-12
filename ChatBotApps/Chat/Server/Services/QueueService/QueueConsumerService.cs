using Chat.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Services.QueueService
{
    public class QueueConsumerService : IQueueConsumerService
    {
        const string HOST_NAME = "localhost";
        const string QUEUE_MESSAGE = "queue_message";
        
        private readonly IHubContext<ChatHub> _chatHub;
        private IConnection _connection;
        private IModel _channel;

        public QueueConsumerService(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }

        public void Run()
        {
            var factory = new ConnectionFactory() { HostName = HOST_NAME };
            
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QUEUE_MESSAGE, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            _channel.BasicConsume(QUEUE_MESSAGE, autoAck: true, consumer: consumer);
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
