using ChatAPI.DAL.Models;

namespace ChatAPI.BLL.Interfaces
{
    public interface IMessagesService
    {
        Task<Message> CreateAsync(Message message);

        Task<IEnumerable<Message>> GetForChatAsync(int chatId);

        Task<Message> GetAsync(int id);

        Task RemoveAsync(int id, int userId);

        Task<Message> UpdateAsync(int id, Message updated);
    }
}