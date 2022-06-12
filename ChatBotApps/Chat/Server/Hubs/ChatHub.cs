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

            var chatMessage = await _messageProcessingService.ProcessMessage(message, user, dateTime);
            if (chatMessage.User == "BOT")
            {
                await Clients.All.SendAsync("ReceiveMessage", chatMessage.Message, chatMessage.User, chatMessage.DateTime);
            }
        }
    }
}
