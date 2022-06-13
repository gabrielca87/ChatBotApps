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
        const string STOCK_COMMAND = "/stock=";

        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IQueuePublisherService _queuePublisherService;

        private bool _botHasSomethingToSay;

        public bool BotHasSomethingToSay { get { return _botHasSomethingToSay; } }

        public MessageProcessingService(IChatMessageRepository chatMessageRepository, IQueuePublisherService queuePublisherService)
        {
            _chatMessageRepository = chatMessageRepository;
            _queuePublisherService = queuePublisherService;
        }

        public async Task<string> ProcessMessage(string message, string user, DateTime dateTime)
        {
            _botHasSomethingToSay = true;

            try
            {
                if (IsCommand(message))
                {
                    string command = GetCommand(message);
                    if (string.IsNullOrEmpty(command) || !IsStockCommand(command))
                    {
                        return "I don't understand this command.";
                    }

                    string stock = GetStockCode(message);
                    if (string.IsNullOrEmpty(stock))
                    {
                        return "The command is valid but the stock code is missing.";
                    }

                    _queuePublisherService.Publish(stock);

                    return "I will quickly return the response for this command.";
                }

                var chatMessage = new ChatMessage
                {
                    Message = message,
                    User = user,
                    DateTime = dateTime
                };

                await _chatMessageRepository.CreateAsync(chatMessage);

                _botHasSomethingToSay = false;
                return null;
            }
            catch (Exception)
            {
                return "There was an error processing the message.";
                //TODO: log
            }
        }

        private bool IsStockCommand(string command)
        {
            return command.StartsWith(STOCK_COMMAND);
        }

        private bool IsCommand(string message)
        {
            return message.StartsWith("/");
        }

        private string GetCommand(string message)
        {
            if (message.Contains("="))
            {
                return message.Substring(0, message.IndexOf("=") + 1);
            }

            return null;
        }

        private string GetStockCode(string message)
        {
            return message.Remove(0, STOCK_COMMAND.Length);
        }
    }
}
