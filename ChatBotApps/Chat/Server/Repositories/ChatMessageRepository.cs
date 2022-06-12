using Chat.Server.Data;
using Chat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Server.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ChatAppContext _context;

        public ChatMessageRepository(ChatAppContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ChatMessage chatMessage)
        {
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();
        }
    }
}
