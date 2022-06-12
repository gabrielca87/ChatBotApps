using Chat.Server.Models;
using Chat.Server.Repositories;
using Chat.Server.Services.QueueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Services.MessageProcessing
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private const string STOCK_COMMAND = "/stock=";

        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IQueuePublisherService _queuePublisherService;
        private bool _canSendBackToClients = false;

        public bool CanSendBackToClients { get => _canSendBackToClients; }

        public MessageProcessingService(IChatMessageRepository chatMessageRepository, IQueuePublisherService queuePublisherService)
        {
            _chatMessageRepository = chatMessageRepository;
            _queuePublisherService = queuePublisherService;
        }

        public async Task<ChatMessage> ProcessMessage(string message, string user, DateTime dateTime)
        {
            ChatMessage chatMessage;

            if (IsStockCommand(message))
            {
                _canSendBackToClients = false;

                chatMessage = new ChatMessage
                { 
                    Message = "I will briefly return the response for your command.", 
                    User = "BOT", 
                    DateTime = DateTime.Now
                };

                string stock = GetStockValue(message);

                _queuePublisherService.Publish(stock);
            }
            else
            {
                _canSendBackToClients = true;

                chatMessage = new ChatMessage
                { 
                    Message = message, 
                    User = user, 
                    DateTime = dateTime
                };

                await _chatMessageRepository.CreateAsync(chatMessage);
            }

            return chatMessage;
        }

        private bool IsStockCommand(string message)
        {
            return message.StartsWith(STOCK_COMMAND);
        }

        private string GetStockValue(string message)
        {
            return message.Remove(0, STOCK_COMMAND.Length);
        }
    }
}
