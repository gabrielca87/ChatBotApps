using Bot.Config;
using Bot.Services.Queue;
using Bot.Services.Stock;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Bot
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            SetupServiceProvider();

            var queueService = _serviceProvider.GetService<IQueueService>();
            queueService.Run();
        }

        private static void SetupServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json", true, true)
                .Build();

            var services = new ServiceCollection();
            services.AddTransient<IStockService, StockService>();
            services.AddHttpClient<StockService>();
            services.AddTransient<IQueueService, QueueService>();

            services.Configure<RabbitMQSettings>(options => configuration.GetSection("RabbitMQSettings").Bind(options));
            services.Configure<StockServiceSettings>(options => configuration.GetSection("StockServiceSettings").Bind(options));

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
