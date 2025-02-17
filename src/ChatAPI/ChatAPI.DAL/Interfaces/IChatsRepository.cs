﻿using ChatAPI.DAL.Models;

namespace ChatAPI.DAL.Interfaces
{
    public interface IChatsRepository : IRepositoryBase<Chat>
    {
        Task<IEnumerable<Chat>> GetWhereUserIsAdminAsync(int userId);
    }
}