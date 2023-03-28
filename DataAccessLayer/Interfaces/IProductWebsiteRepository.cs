using DataAccessLayer.Models;
using System;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IProductWebsiteRepository
    {
        Task<ProductWebsite> AddAsync(ProductWebsite productWebsite);

        Task<ProductWebsite> GetAsync(Int32 productId, Int32 websiteId);

        Task<ProductWebsite> UpdateAsync(ProductWebsite productWebsite);

        Task<Int32> DeleteAsync(Int32 productId, Int32 websiteId);
    }
}