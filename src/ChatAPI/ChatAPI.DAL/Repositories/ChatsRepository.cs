using ChatAPI.DAL.Data;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.DAL.Repositories
{
    public class ChatsRepository(ChatDbContext context) : RepositoryBase<Chat>(context), IChatsRepository
    {
        public async Task<IEnumerable<Chat>> GetAllForUserAsync(int userId)
        {
            return await GetListWhereAsync(x => x.AdminId == userId);
        }
    }
}