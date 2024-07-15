using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;
using ChatAPI.DAL.Repositories;

namespace ChatAPI.BLL.Services
{
    public class MessagesService(IMessagesRepository messagesRepository) : IMessagesService
    {
        public async Task<Message> CreateAsync(Message message)
        {
            await messagesRepository.CreateAsync(message);

            return message;
        }

        public async Task<IEnumerable<Message>> GetForChatAsync(int chatId)
        {
            return await messagesRepository.GetListWhereAsync(x => x.ChatId == chatId);
        }

        public async Task<Message> GetAsync(int id)
        {
            var message = await messagesRepository.GetByIdAsync(id);

            if (message is null)
            {
                throw new ArgumentException("Message not found");
            }

            return message;
        }

        public async Task RemoveAsync(int id, int userId)
        {
            var message = await messagesRepository.GetByIdAsync(id);

            if (message is null || message.AuthorId != userId)
            {
                throw new ArgumentException("Message not found");
            }

            await messagesRepository.RemoveAsync(message);
        }

        public async Task<Message> UpdateAsync(int id, Message updated)
        {
            var message = await messagesRepository.GetByIdAsync(id);
            
            if (message is null)
            {
                throw new ArgumentException("Chat not found");
            }

            updated.Id = message.Id;

            await messagesRepository.UpdateAsync(updated);

            return updated;
        }
    }

}