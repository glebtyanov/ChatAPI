using ChatAPI.DAL.Models;

namespace ChatAPI.DAL.Interfaces
{
    public interface IChatsRepository : IRepositoryBase<Chat>
    {
        Task<IEnumerable<Chat>> GetAllForUserAsync(int userId);
    }
}