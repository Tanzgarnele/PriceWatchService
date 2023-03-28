using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IWebsiteRepository
    {
        Task<IEnumerable<Website>> GetAllAsync();

        Task<Website> GetByIdAsync(Int32 id);

        Task<Website> AddAsync(Website website);

        Task<Int32> DeleteAsync(Int32 id);

        Task<Website> UpdateAsync(Website website);
    }
}