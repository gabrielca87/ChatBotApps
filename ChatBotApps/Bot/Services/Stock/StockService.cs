using Bot.Config;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bot.Services.Stock
{
    public class StockService : IStockService
    {
        const string STOCK_MESSAGE_SUCCESS = "{0} quote is ${1} per share.";
        const string STOCK_MESSAGE_ERROR = "I got an error trying to get a stock value for '{0}'.";
        
        private readonly HttpClient _httpClient;
        private readonly IOptions<StockServiceSettings> _configuration;

        public StockService(HttpClient httpClient, IOptions<StockServiceSettings> configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetStockQuoteMessage(string stock)
        {
            string messageError = string.Format(STOCK_MESSAGE_ERROR, stock);
            string messageSuccess;

            try
            {
                string stockServiceUri = string.Format(_configuration.Value.Uri, stock);
                
                string csvContent = await _httpClient.GetStringAsync(stockServiceUri);

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
