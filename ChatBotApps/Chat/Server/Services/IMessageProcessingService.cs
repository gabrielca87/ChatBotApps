using System;
using System.Threading.Tasks;

namespace Chat.Server.Services
{
    public interface IMessageProcessingService
    {
        bool CanSendBackToClients { get; }
        Task ProcessMessage(string message, string user, DateTime dateTime);
    }
}
