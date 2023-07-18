using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.DataAccessLayer.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserProductRepository : IUserProductRepository
    {
        private readonly IDbConnection dbConnection;

        public UserProductRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<UserProduct> AddAsync(UserProduct userProduct)
        {
            if (userProduct == null)
            {
                throw new ArgumentNullException(nameof(userProduct));
            }

            if (userProduct.UserId <= 0)
            {
                throw new ArgumentException("Invalid UserId.");
            }

            if (userProduct.ProductId <= 0)
            {
                throw new ArgumentException("Invalid ProductId.");
            }

            if (userProduct.ThresholdPrice <= 0)
            {
                throw new ArgumentException("ThresholdPrice must be greater than zero.");
            }

            String sql = "INSERT INTO UserProducts (UserId, ProductId, ThresholdPrice) " +
                         "VALUES (@UserId, @ProductId, @ThresholdPrice)";

            await this.dbConnection.ExecuteAsync(sql, userProduct);

            return userProduct;
        }


        public async Task<UserProduct> UpdateAsync(UserProduct userProduct)
        {
            if (userProduct == null)
            {
                throw new ArgumentNullException(nameof(userProduct));
            }

            if (userProduct.UserId <= 0)
            {
                throw new ArgumentException("Invalid UserId.");
            }

            if (userProduct.ProductId <= 0)
            {
                throw new ArgumentException("Invalid ProductId.");
            }

            if (userProduct.ThresholdPrice <= 0)
            {
                throw new ArgumentException("ThresholdPrice must be greater than zero.");
            }

            String sql = "UPDATE UserProducts SET UserId = @UserId, ProductId = @ProductId, ThresholdPrice = @ThresholdPrice " +
                         "WHERE Id = @Id";

            await this.dbConnection.ExecuteAsync(sql, userProduct);

            return userProduct;
        }
    }
}