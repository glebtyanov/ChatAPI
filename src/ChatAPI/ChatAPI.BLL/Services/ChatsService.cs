using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;
using ChatAPI.DAL.Repositories;

namespace ChatAPI.BLL.Services
{
    public class ChatsService(IChatsRepository chatsRepository, IMessagesRepository messagesRepository) : IChatsService
    {
        public async Task<Chat> CreateAsync(Chat chat)
        {
            await chatsRepository.CreateAsync(chat);

            return chat;
        }

        public async Task<IEnumerable<Chat>> GetForUserAsync(int userId)
        {
            return await chatsRepository.GetAllForUserAsync(userId);
        }

        public async Task<Chat> GetAsync(int id)
        {
            var chat = await chatsRepository.GetByIdAsync(id);

            if (chat is null)
            {
                throw new ArgumentException("Chat not found");
            }

            return chat;
        }

        public async Task RemoveAsync(int id, int userId)
        {
            var chat = await chatsRepository.GetByIdAsync(id);

            if (chat is null || chat.AdminId != userId)
            {
                throw new ArgumentException("Chat not found");
            }

            await chatsRepository.RemoveAsync(chat);
        }

        public async Task<Chat> UpdateAsync(int id, Chat updated)
        {
            var chat = await chatsRepository.GetByIdAsync(id);
            
            if (chat is null)
            {
                throw new ArgumentException("Chat not found");
            }

            updated.Id = chat.Id;

            await chatsRepository.UpdateAsync(updated);

            return updated;
        }

        public async Task<IEnumerable<Chat>> GetByName(string name)
        {
            return await chatsRepository.GetListWhereAsync(x => x.Name == name);
        }

        public async Task<bool> ExistsWithName(string name)
        {
            return await chatsRepository.ExistsAsync(x => x.Name == name);
        }

        public async Task<Message> SendMessageAsync(int chatId, int authorId, string text)
        {
            var message = new Message { ChatId = chatId, AuthorId = authorId, Text = text };
            await messagesRepository.CreateAsync(message);
            return message;
        }
    }

}