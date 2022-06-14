using Chat.Server.Models;
using System.Threading.Tasks;

namespace Chat.Server.Repositories
{
    public interface IChatMessageRepository
    {
        Task CreateAsync(ChatMessage chatMessage);
    }
}
