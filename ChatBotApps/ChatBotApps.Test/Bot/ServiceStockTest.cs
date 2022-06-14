using Bot.Config;
using Bot.Services.Stock;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ChatBotApps.Test.Bot
{
    public class ServiceStockTest
    {
        [Fact]
        public async Task GetStockQuoteMessage_Stock_ExpectedMessage()
        {
            //Arrange
            string stock = "aapl.us";
            string csvContent = "Symbol,Date,Time,Open,High,Low,Close,Volume\nAAPL.US,2022-06-09,22:00:10,147.08,147.95,142.53,142.64,69472976";
            string uri = "https://stooq.com/q/l/?s={0}&f=sd2t2ohlcv&h&e=csv";

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(csvContent)
                }).Verifiable();

            var stockServiceSettings = new StockServiceSettings { Uri = uri };
            var optionsMock = new Mock<IOptions<StockServiceSettings>>();
            optionsMock.Setup(x => x.Value).Returns(stockServiceSettings);

            var httpClient = new HttpClient(handlerMock.Object);
            var stockService = new StockService(httpClient, optionsMock.Object);

            //Act
            var stockQuoteMessage = await stockService.GetStockQuoteMessage(stock);

            //Assert
            Assert.Contains("quote is", stockQuoteMessage);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }

        [Theory]
        [InlineData("aapl.us", "Symbol,Date,Time,Open,High,Low,Close,Volume", "https://stooq.com/q/l/")]
        [InlineData("aapl.us", "Symbol,Date,Time,Open,High,Low,Close,Volume\nAAPL.US,2022-06-09,22:00:10,147.08", "https://stooq.com/q/l/")]
        [InlineData("aapl.us", "Symbol,Date,Time,Open,High,Low,Close,Volume\nAAPL.US,2022-06-09,22:00:10,N/D,N/D,N/D,N/D,N/D", "https://stooq.com/q/l/")]
        public async Task GetStockQuoteMessage_Stock_ErrorMessage(string stock, string csvContent, string uri)
        {
            //Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(csvContent)
                }).Verifiable();

            var stockServiceSettings = new StockServiceSettings { Uri = uri };
            var optionsMock = new Mock<IOptions<StockServiceSettings>>();
            optionsMock.Setup(x => x.Value).Returns(stockServiceSettings);

            var httpClient = new HttpClient(handlerMock.Object);
            var stockService = new StockService(httpClient, optionsMock.Object);

            //Act
            var stockQuoteMessage = await stockService.GetStockQuoteMessage(stock);

            //Assert
            Assert.StartsWith("I got an error trying to get a stock value for ", stockQuoteMessage);

            handlerMock.Protected().Verify(
               "SendAsync",
               Times.Exactly(1),
               ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
               ItExpr.IsAny<CancellationToken>());
        }
    }
}
