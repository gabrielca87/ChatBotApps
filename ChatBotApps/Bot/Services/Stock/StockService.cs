using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.Services.Stock
{
    public class StockService : IStockService
    {
        const string STOCK_SERVICE_URL = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";
        const string STOCK_MESSAGE_SUCCESS = "{0} quote is ${1} per share.";
        const string STOCK_MESSAGE_ERROR = "We could not find quotes for stock {0}.";

        public async Task<string> GetStockQuoteMessage(string stock)
        {
            string stockQuoteMessage = string.Empty;

            try
            {
                var client = GetClient();

                string stockServiceUrl = string.Format(STOCK_SERVICE_URL, stock);
                
                string csvContent = await client.GetStringAsync(stockServiceUrl);

                string[] rows = csvContent.Split("\n");
                if (rows.Length < 1)
                {
                    stockQuoteMessage = string.Format(STOCK_MESSAGE_ERROR, stock);
                    throw new Exception($"{ stockQuoteMessage } The CSV file does not have the lines expected.");
                }

                string[] columns = rows[1].Split(",");
                if (columns.Length < 8)
                {
                    stockQuoteMessage = string.Format(STOCK_MESSAGE_ERROR, stock);
                    throw new Exception($"{ stockQuoteMessage } The CSV file does not have the columns expected.");
                }

                string symbol = columns[0];
                string close = columns[6];

                stockQuoteMessage = string.Format(STOCK_MESSAGE_SUCCESS, symbol, close);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return stockQuoteMessage;
        }

        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            
            return client;
        }
    }
}
