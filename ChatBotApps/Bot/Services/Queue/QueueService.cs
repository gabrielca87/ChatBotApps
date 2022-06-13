using Bot.Config;
using Bot.Services.Stock;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.Queue
{
    public class QueueService: IQueueService
    {
        private readonly IStockService _stockService;
        private readonly IOptions<RabbitMQSettings> _configuration;
        private IConnection _connection;
        private IModel _channel;

        public QueueService(IStockService stockService, IOptions<RabbitMQSettings> configuration)
        {
            _stockService = stockService;
            _configuration = configuration;
        }

        public void Run()
        {
            var factory = new ConnectionFactory() { HostName = _configuration.Value.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration.Value.QueueStock, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                Console.WriteLine($"Received '{ message }'.");

                Task.Run(async () =>
                {
                    var stockQuoteMessage = await _stockService.GetStockQuoteMessage(message);
                    Publish(stockQuoteMessage);
                }).Wait();
            };

            _channel.BasicConsume(_configuration.Value.QueueStock, autoAck: true, consumer: consumer);

            Console.WriteLine($"BOT started to consume queue: '{ _configuration.Value.QueueStock }'.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private void Publish(string stockQuoteMessage)
        {
            var body = Encoding.UTF8.GetBytes(stockQuoteMessage);

            _channel.BasicPublish(exchange: string.Empty, routingKey: _configuration.Value.QueueMessage, basicProperties: null, body: body);

            Console.WriteLine($"Sent '{ stockQuoteMessage }' to queue '{ _configuration.Value.QueueMessage }'.");
        }
    }
}
