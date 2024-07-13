﻿using ChatAPI.DAL.Data;
using ChatAPI.DAL.Models;

namespace ChatAPI.DAL.Repositories
{
    public class UsersRepository(ChatDbContext context) : RepositoryBase<User>(context);
}