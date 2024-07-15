using ChatAPI.DAL.Data;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.DAL.Repositories
{
    public class MessagesRepository(ChatDbContext context) : RepositoryBase<Message>(context), IMessagesRepository
    {
        public new async Task<Message?> GetByIdAsync(int id)
        {
            return await Context.Messages
                .Include(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}