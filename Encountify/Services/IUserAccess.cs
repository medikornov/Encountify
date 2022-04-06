﻿using Encountify.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Encountify.Services
{
    public interface IUserAccess
    {
        Task<bool> AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task<User> GetAsync(int id);
        Task<IEnumerable<User>> GetAllAsync(bool forceRefresh = false);
        Task<Token> AuthorizeAsync(string email, string password);
    }
}
