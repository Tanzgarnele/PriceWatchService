using DataAccessLayer.Models.DataAccessLayer.Models;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserProductRepository
    {
        Task<UserProduct> AddAsync(UserProduct userProduct);

        Task<UserProduct> UpdateAsync(UserProduct userProduct);
    }
}