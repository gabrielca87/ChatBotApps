using Chat.Server.Models;
using Chat.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Services
{
    public class MessageProcessingService : IMessageProcessingService
    {
        private const string STOCK = "/stock=";
        private readonly IChatMessageRepository _chatMessageRepository;
        private bool _canSendBackToClients = false;

        public bool CanSendBackToClients { get => _canSendBackToClients; }

        public MessageProcessingService(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task ProcessMessage(string message, string user, DateTime dateTime)
        {
            if (IsMessageStockCommand(message))
            {
                _canSendBackToClients = false;

                //call to bot
            }
            else
            {
                _canSendBackToClients = true;

                var chatMessage = new ChatMessage { Message = message, User = user, DateTime = dateTime };

                await _chatMessageRepository.CreateAsync(chatMessage);
            }
        }

        private bool IsMessageStockCommand(string message)
        {
            return message.StartsWith(STOCK);
        }
    }
}
