using DataAccessLayer.Models;
using System;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Int32 id);

        Task<Product> AddAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task<Int32> DeleteAsync(Int32 id);
    }
}