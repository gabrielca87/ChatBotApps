using Bot.Services.Queue;
using Bot.Services.Stock;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            var services = new ServiceCollection();
            services.AddTransient<IStockService, StockService>();
            services.AddHttpClient<StockService>();
            services.AddTransient<IQueueService, QueueService>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
