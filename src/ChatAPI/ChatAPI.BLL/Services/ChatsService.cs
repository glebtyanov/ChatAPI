using ChatAPI.BLL.Exceptions;
using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;

namespace ChatAPI.BLL.Services
{
    public class ChatsService(
        IChatsRepository chatsRepository,
        IMessagesRepository messagesRepository,
        IUsersRepository usersRepository) : IChatsService
    {
        public async Task<Chat> CreateAsync(Chat chat)
        {
            if (await ExistsWithNameAsync(chat.Name))
            {
                throw new BadRequestException();
            }

            await chatsRepository.CreateAsync(chat);

            return chat;
        }

        public async Task<IEnumerable<Chat>> GetWhereUserIsAdminAsync(int userId)
        {
            if (!await usersRepository.ExistsAsync(u => u.Id == userId))
            {
                throw new NotFoundException();
            }

            return await chatsRepository.GetWhereUserIsAdminAsync(userId);
        }

        public async Task<Chat> GetAsync(int id)
        {
            var chat = await chatsRepository.GetByIdAsync(id);

            if (chat is null)
            {
                throw new NotFoundException();
            }

            return chat;
        }

        public async Task RemoveAsync(int id, int userId)
        {
            var chat = await chatsRepository.GetByIdAsync(id);

            if (chat is null)
            {
                throw new NotFoundException();
            }

            if (chat.AdminId != userId)
            {
                throw new BadRequestException();
            }

            await chatsRepository.RemoveAsync(chat);
        }

        public async Task<Chat> UpdateAsync(int id, Chat updated)
        {
            var chat = await chatsRepository.GetByIdAsync(id);

            if (chat is null)
            {
                throw new NotFoundException();
            }

            updated.Id = chat.Id;

            await chatsRepository.UpdateAsync(updated);

            return updated;
        }

        public async Task<Chat> GetByNameAsync(string name)
        {
            var chat = await chatsRepository.GetWhereAsync(x => x.Name == name);

            if (chat is null)
            {
                throw new NotFoundException();
            }

            return chat;
        }

        public async Task<bool> ExistsWithNameAsync(string name)
        {
            return await chatsRepository.ExistsAsync(x => x.Name == name);
        }

        public async Task<Message> SendMessageAsync(int chatId, int authorId, string text)
        {
            var message = new Message { ChatId = chatId, AuthorId = authorId, Text = text };
            await messagesRepository.CreateAsync(message);
            return (await messagesRepository.GetByIdAsync(message.Id))!;
        }
    }
}