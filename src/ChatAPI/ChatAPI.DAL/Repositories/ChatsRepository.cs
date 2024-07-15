using ChatAPI.DAL.Data;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;

namespace ChatAPI.DAL.Repositories
{
    public class ChatsRepository(ChatDbContext context) : RepositoryBase<Chat>(context), IChatsRepository
    {
        public async Task<IEnumerable<Chat>> GetWhereUserIsAdminAsync(int userId)
        {
            return await GetListWhereAsync(x => x.AdminId == userId);
        }
    }
}