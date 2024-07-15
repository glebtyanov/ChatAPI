using ChatAPI.DAL.Models;

namespace ChatAPI.BLL.Interfaces
{
    public interface IChatsService
    {
        Task<Chat> CreateAsync(Chat chat);

        Task<Message> SendMessageAsync(int chatId, int authorId, string text);

        Task<IEnumerable<Chat>> GetWhereUserIsAdminAsync(int userId);

        Task<Chat> GetAsync(int id);

        Task RemoveAsync(int id, int userId);

        Task<Chat> UpdateAsync(int id, Chat updated);

        Task<Chat> GetByNameAsync(string name);
        Task<bool> ExistsWithNameAsync(string name);
    }
}