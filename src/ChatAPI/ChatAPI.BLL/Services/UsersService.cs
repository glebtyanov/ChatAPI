using ChatAPI.BLL.Exceptions;
using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;

namespace ChatAPI.BLL.Services
{
    public class UsersService(IUsersRepository usersRepository) : IUsersService
    {
        public async Task<User> RegisterAsync(User user)
        {
            await usersRepository.CreateAsync(user);

            return user;
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await usersRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new NotFoundException();
            }

            return user;
        }

        public async Task RemoveAsync(int id)
        {
            var user = await usersRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new NotFoundException();
            }

            await usersRepository.RemoveAsync(user);
        }

        public async Task<User> UpdateAsync(int id, User updated)
        {
            var user = await usersRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new NotFoundException();
            }

            updated.Id = user.Id;

            await usersRepository.UpdateAsync(updated);

            return updated;
        }
    }
}