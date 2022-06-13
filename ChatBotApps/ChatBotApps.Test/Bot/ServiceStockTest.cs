using Bot.Services.Stock;
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

            var httpClient = new HttpClient(handlerMock.Object);
            var stockService = new StockService(httpClient);

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
        [InlineData("aapl.us", "Symbol,Date,Time,Open,High,Low,Close,Volume")]
        [InlineData("aapl.us", "Symbol,Date,Time,Open,High,Low,Close,Volume\nAAPL.US,2022-06-09,22:00:10,147.08")]
        [InlineData("aapl.us", "Symbol,Date,Time,Open,High,Low,Close,Volume\nAAPL.US,2022-06-09,22:00:10,N/D,N/D,N/D,N/D,N/D")]
        public async Task GetStockQuoteMessage_Stock_ErrorMessage(string stock, string csvContent)
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

            var httpClient = new HttpClient(handlerMock.Object);
            var stockService = new StockService(httpClient);

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
