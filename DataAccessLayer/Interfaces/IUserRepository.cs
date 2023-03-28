using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> GetByIdAsync(Int32 id);

        Task<User> AddAsync(User user);

        Task<User> UpdateAsync(User user);

        Task<Int32> DeleteAsync(Int32 id);

        Task<Boolean> ChangePasswordAsync(Int32 userId, String oldPassword, String newPassword);
    }
}