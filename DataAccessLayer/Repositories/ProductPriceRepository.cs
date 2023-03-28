using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductPriceRepository : IProductPriceRepository
    {
        private readonly IDbConnection dbConnection;

        public ProductPriceRepository(IDbConnection db)
        {
            this.dbConnection = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ProductPrice> GetByIdAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            String sql = "SELECT * FROM product_prices WHERE Id = @Id";

            return await this.dbConnection.QueryFirstOrDefaultAsync<ProductPrice>(sql, new { Id = id });
        }

        public async Task<IEnumerable<ProductPrice>> GetByProductIdAsync(Int32 productId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid productId.");
            }

            String sql = "SELECT * FROM product_prices WHERE ProductId = @ProductId";

            return await this.dbConnection.QueryAsync<ProductPrice>(sql, new { ProductId = productId });
        }

        public async Task<IEnumerable<ProductPrice>> GetByWebsiteIdAsync(Int32 websiteId)
        {
            if (websiteId <= 0)
            {
                throw new ArgumentException("Invalid websiteId.");
            }

            String sql = "SELECT * FROM product_prices WHERE WebsiteId = @WebsiteId";

            return await this.dbConnection.QueryAsync<ProductPrice>(sql, new { WebsiteId = websiteId });
        }

        public async Task AddAsync(ProductPrice productPrice)
        {
            String sql = "INSERT INTO product_prices (ProductId, WebsiteId, Price, LastPrice, CurrentPrice, UpdatedAt) " +
                         "VALUES (@ProductId, @WebsiteId, @Price, @LastPrice, @CurrentPrice, @UpdatedAt)";

            await this.dbConnection.ExecuteAsync(sql, productPrice);
        }

        public async Task<ProductPrice> UpdateAsync(ProductPrice productPrice)
        {
            if (productPrice.Id <= 0)
            {
                throw new ArgumentException("Invalid Id.");
            }

            String sql = "UPDATE product_prices SET ProductId = @ProductId, WebsiteId = @WebsiteId, " +
                            "Price = @Price, LastPrice = @LastPrice, CurrentPrice = @CurrentPrice, " +
                            "UpdatedAt = @UpdatedAt WHERE Id = @Id";

            await this.dbConnection.ExecuteAsync(sql, productPrice);

            return productPrice;
        }

        public async Task<Int32> DeleteAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id value.");
            }

            String sql = "DELETE FROM product_prices WHERE Id = @Id";

            return await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}