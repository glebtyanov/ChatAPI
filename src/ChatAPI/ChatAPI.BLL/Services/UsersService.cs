using ChatAPI.BLL.Interfaces;
using ChatAPI.DAL.Interfaces;
using ChatAPI.DAL.Models;
using ChatAPI.DAL.Repositories;

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
                throw new ArgumentException("User not found");
            }

            return user;
        }

        public async Task RemoveAsync(int id)
        {
            var user = await usersRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new ArgumentException("User not found");
            }

            await usersRepository.RemoveAsync(user);
        }

        public async Task<User> UpdateAsync(int id, User updated)
        {
            var user = await usersRepository.GetByIdAsync(id);
            
            if (user is null)
            {
                throw new ArgumentException("User not found");
            }

            updated.Id = user.Id;

            await usersRepository.UpdateAsync(updated);

            return updated;
        }
    }

}