using ChatAPI.DAL.Models;

namespace ChatAPI.BLL.Interfaces
{
    public interface IUsersService
    {
        Task<User> RegisterAsync(User user);

        Task<User> GetAsync(int id);

        Task RemoveAsync(int id);

        Task<User> UpdateAsync(int id, User updated);
    }
}