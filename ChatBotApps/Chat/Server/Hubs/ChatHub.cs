using Chat.Server.Services.MessageProcessing;
using Chat.Server.Services.QueueService;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Chat.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageProcessingService _messageProcessingService;

        public ChatHub(IMessageProcessingService messageProcessingService)
        {
            _messageProcessingService = messageProcessingService;
        }

        public async Task SendMessage(string message, string user, DateTime dateTime)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, user, dateTime);

            var messageProcessed = await _messageProcessingService.ProcessMessage(message, user, dateTime);
            if (_messageProcessingService.BotHasSomethingToSay)
            {
                await Clients.All.SendAsync("ReceiveMessage", messageProcessed, "BOT", dateTime);
            }
        }
    }
}
