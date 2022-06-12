using Bot.Services.Stock;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    class Program
    {
        const string HOST_NAME = "localhost";
        const string QUEUE_STOCK = "queue_stock";
        const string QUEUE_MESSAGE = "queue_message";

        private static IServiceProvider _serviceProvider;
        private static HubConnection _connection;

        static void Main(string[] args)
        {
            SetupServiceProvider();

            var factory = new ConnectionFactory() { HostName = HOST_NAME };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: QUEUE_STOCK, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    //var dynamic = JsonConvert.DeserializeObject<dynamic>(message);
                    //int connectionId = dynamic.ConnectionId;
                    //string stockCode = dynamic.StockCode;

                    //Console.WriteLine($"From 'queue_stock': Connection Id: { connectionId }, Stock Code: { stockCode }");

                    var stockService = _serviceProvider.GetService<IStockService>();

                    Task.Run(async () =>
                    {
                        var stockQuoteMessage = await stockService.GetStockQuoteMessage(message);
                        Console.WriteLine(stockQuoteMessage);

                        var body2 = Encoding.UTF8.GetBytes(stockQuoteMessage);

                        channel.BasicPublish(exchange: string.Empty, routingKey: QUEUE_MESSAGE, basicProperties: null, body: body2);

                        Console.WriteLine($"Sent to 'queue_message': { stockQuoteMessage }");
                    }).Wait();
                };

                channel.BasicConsume(QUEUE_STOCK, autoAck: true, consumer: consumer);

                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
        }

        private static void SetupServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IStockService, StockService>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
