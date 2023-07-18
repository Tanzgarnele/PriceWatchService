using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Dictionary<String, Object>>> GetAllAsync();

        Task<Dictionary<String, Object>> GetByIdAsync(Int32 id);

        Task<Int32> AddAsync(User user);

        Task<Int32> DeleteAsync(Int32 id);

        Task<Boolean> ChangePasswordAsync(Int32 userId, String oldPassword, String newPassword);
    }
}