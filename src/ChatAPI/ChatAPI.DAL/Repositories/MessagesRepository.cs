using ChatAPI.DAL.Data;
using ChatAPI.DAL.Models;

namespace ChatAPI.DAL.Repositories
{
    public class MessagesRepository(ChatDbContext context) : RepositoryBase<Message>(context);
}