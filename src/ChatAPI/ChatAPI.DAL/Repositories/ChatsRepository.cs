using ChatAPI.DAL.Data;
using ChatAPI.DAL.Models;

namespace ChatAPI.DAL.Repositories
{
    public class ChatsRepository(ChatDbContext context) : RepositoryBase<Chat>(context);
}