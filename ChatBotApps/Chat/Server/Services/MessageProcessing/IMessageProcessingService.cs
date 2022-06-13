using Chat.Server.Models;
using System;
using System.Threading.Tasks;

namespace Chat.Server.Services.MessageProcessing
{
    public interface IMessageProcessingService
    {
        bool BotHasSomethingToSay { get; }
        Task<string> ProcessMessage(string message, string user, DateTime dateTime);
    }
}
