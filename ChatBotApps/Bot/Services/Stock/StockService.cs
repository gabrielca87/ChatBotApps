using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.Services.Stock
{
    public class StockService : IStockService
    {
        const string STOCK_SERVICE_URL = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";
        const string STOCK_MESSAGE_SUCCESS = "{0} quote is ${1} per share.";
        const string STOCK_MESSAGE_ERROR = "I got an error trying to get a stock value for '{0}'.";

        public async Task<string> GetStockQuoteMessage(string stock)
        {
            string messageError = string.Format(STOCK_MESSAGE_ERROR, stock);
            string messageSuccess;

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();

                string stockServiceUrl = string.Format(STOCK_SERVICE_URL, stock);
                
                string csvContent = await client.GetStringAsync(stockServiceUrl);

                string[] rows = csvContent.Split("\n");
                if (rows.Length < 1)
                {
                    return messageError;
                    //TODO: log: The CSV file does not have the lines expected.");
                }

                string[] columns = rows[1].Split(",");
                if (columns.Length < 8)
                {
                    return messageError;
                    //TODO: log: The CSV file does not have the columns expected.");
                }

                string symbol = columns[0];
                string close = columns[6];
                if (close == "N/D")
                {
                    return messageError;
                    //TODO: log: The CSV file does not have valid values.");
                }

                messageSuccess = string.Format(STOCK_MESSAGE_SUCCESS, symbol, close);
                return messageSuccess;
            }
            catch (Exception ex)
            {
                //TODO: log
                return messageError;
            }
        }
    }
}
