using Bot.Services.Stock;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.Queue
{
    public class QueueService: IQueueService
    {
        const string HOST_NAME = "localhost";
        const string QUEUE_STOCK = "queue_stock";
        const string QUEUE_MESSAGE = "queue_message";

        private readonly IStockService _stockService;        
        private IConnection _connection;
        private IModel _channel;

        public QueueService(IStockService stockService)
        {
            _stockService = stockService;
        }

        public void Run()
        {
            var factory = new ConnectionFactory() { HostName = HOST_NAME };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QUEUE_STOCK, durable: false, exclusive: false, autoDelete: false, arguments: null);

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

            _channel.BasicConsume(QUEUE_STOCK, autoAck: true, consumer: consumer);

            Console.WriteLine($"BOT started to consume queue: '{ QUEUE_STOCK }'.");
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private void Publish(string stockQuoteMessage)
        {
            var body = Encoding.UTF8.GetBytes(stockQuoteMessage);

            _channel.BasicPublish(exchange: string.Empty, routingKey: QUEUE_MESSAGE, basicProperties: null, body: body);

            Console.WriteLine($"Sent '{ stockQuoteMessage }' to queue '{ QUEUE_MESSAGE }'.");
        }
    }
}
