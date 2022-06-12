using Chat.Server.Services;
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
            await _messageProcessingService.ProcessMessage(message, user, dateTime);

            if (_messageProcessingService.CanSendBackToClients)
            {
                await Clients.All.SendAsync("ReceiveMessage", message, user, dateTime);
            }
        }
    }
}
