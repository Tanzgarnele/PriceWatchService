using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IProductRepository
    {
        Task<Dictionary<String, Object>> GetByIdAsync(Int32 id);

        Task<IEnumerable<Dictionary<String, Object>>> GetAllAsync();

        Task<Int32> AddAsync(Product product);

        Task<Product> UpdateAsync(Int32 id, Product product);

        Task<Int32> DeleteAsync(Int32 id);
    }
}