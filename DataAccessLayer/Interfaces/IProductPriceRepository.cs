using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IProductPriceRepository
    {
        Task<ProductPrice> GetByIdAsync(Int32 id);

        Task<IEnumerable<ProductPrice>> GetByProductIdAsync(Int32 productId);

        Task AddAsync(ProductPrice productPrice);

        Task<ProductPrice> UpdateAsync(ProductPrice productPrice);

        Task<Int32> DeleteAsync(Int32 id);
    }
}