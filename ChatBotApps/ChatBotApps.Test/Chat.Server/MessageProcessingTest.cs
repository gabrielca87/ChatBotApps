using Chat.Server.Models;
using Chat.Server.Repositories;
using Chat.Server.Services.MessageProcessing;
using Chat.Server.Services.QueueService;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ChatBotApps.Test.Chat.Server
{
    public class MessageProcessingTest
    {

        [Theory]
        [InlineData("/")]
        [InlineData("/stock")]
        [InlineData("/othercommand=aapl.us")]
        public async Task SendMessage_IsInvalidCommand(string message)
        {
            //Arrange
            string user = "gabriel";
            DateTime dateTime = DateTime.Now;

            var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
            var queuePublisherService = new Mock<IQueuePublisherService>();

            var messageProcessingService = new MessageProcessingService(chatMessageRepositoryMock.Object, queuePublisherService.Object);

            //Act
            var messageProcessed = await messageProcessingService.ProcessMessage(message, user, dateTime);

            //Assert
            Assert.Equal("I don't understand this command.", messageProcessed);
            Assert.True(messageProcessingService.BotHasSomethingToSay);
        }

        [Fact]
        public async Task SendMessage_IsCommand_StockCodeMissing()
        {
            //Arrange
            string message = "/stock=";
            string user = "gabriel";
            DateTime dateTime = DateTime.Now;

            var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
            var queuePublisherService = new Mock<IQueuePublisherService>();

            var messageProcessingService = new MessageProcessingService(chatMessageRepositoryMock.Object, queuePublisherService.Object);

            //Act
            var messageProcessed = await messageProcessingService.ProcessMessage(message, user, dateTime);

            //Assert
            Assert.Equal("The command is valid but the stock code is missing.", messageProcessed);
            Assert.True(messageProcessingService.BotHasSomethingToSay);
        }

        [Fact]
        public async Task SendMessage_IsValidCommand_ValidStock()
        {
            //Arrange
            string message = "/stock=aapl.us";
            string user = "gabriel";
            DateTime dateTime = DateTime.Now;

            var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
            var queuePublisherService = new Mock<IQueuePublisherService>();

            var messageProcessingService = new MessageProcessingService(chatMessageRepositoryMock.Object, queuePublisherService.Object);

            //Act
            var messageProcessed = await messageProcessingService.ProcessMessage(message, user, dateTime);

            //Assert
            Assert.Equal("I will quickly return the response for this command.", messageProcessed);
            Assert.True(messageProcessingService.BotHasSomethingToSay);

            queuePublisherService.Verify(x => x.Publish(It.IsAny<string>()));
        }

        [Fact]
        public async Task SendMessage_IsRegularChatMessage()
        {
            //Arrange
            string message = "Hola!";
            string user = "gabriel";
            DateTime dateTime = DateTime.Now;

            var chatMessageRepositoryMock = new Mock<IChatMessageRepository>();
            var queuePublisherService = new Mock<IQueuePublisherService>();

            var messageProcessingService = new MessageProcessingService(chatMessageRepositoryMock.Object, queuePublisherService.Object);

            //Act
            var messageProcessed = await messageProcessingService.ProcessMessage(message, user, dateTime);

            //Assert
            Assert.Null(messageProcessed);
            Assert.False(messageProcessingService.BotHasSomethingToSay);

            chatMessageRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<ChatMessage>()));
        }
    }
}
