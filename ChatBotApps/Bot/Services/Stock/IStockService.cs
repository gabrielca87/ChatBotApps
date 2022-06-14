using System.Threading.Tasks;

namespace Bot.Services.Stock
{
    public interface IStockService
    {
        Task<string> GetStockQuoteMessage(string stock);
    }
}
