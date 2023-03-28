using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductWebsiteRepository : IProductWebsiteRepository
    {
        private readonly IDbConnection dbConnection;

        public ProductWebsiteRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<ProductWebsite> AddAsync(ProductWebsite productWebsite)
        {
            if (productWebsite == null)
            {
                throw new ArgumentNullException(nameof(productWebsite));
            }

            if (productWebsite.ProductId <= 0)
            {
                throw new ArgumentException("Invalid ProductId", nameof(productWebsite.ProductId));
            }

            if (productWebsite.WebsiteId <= 0)
            {
                throw new ArgumentException("Invalid WebsiteId", nameof(productWebsite.WebsiteId));
            }

            String sql = "INSERT INTO product_websites (ProductId, WebsiteId, WebsiteProductId) " +
                         "VALUES (@ProductId, @WebsiteId, @WebsiteProductId); " +
                         "SELECT LAST_INSERT_ROWID();";

            Int32 id = await this.dbConnection.QuerySingleAsync<Int32>(sql, productWebsite);
            productWebsite.Id = id;

            return productWebsite;
        }

        public async Task<ProductWebsite> GetAsync(Int32 productId, Int32 websiteId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid ProductId", nameof(productId));
            }

            if (websiteId <= 0)
            {
                throw new ArgumentException("Invalid WebsiteId", nameof(websiteId));
            }

            String sql = "SELECT * FROM product_websites WHERE ProductId = @ProductId AND WebsiteId = @WebsiteId";

            return await this.dbConnection.QuerySingleOrDefaultAsync<ProductWebsite>(sql, new { ProductId = productId, WebsiteId = websiteId });
        }

        public async Task<ProductWebsite> UpdateAsync(ProductWebsite productWebsite)
        {
            if (productWebsite == null)
            {
                throw new ArgumentNullException(nameof(productWebsite));
            }

            if (productWebsite.ProductId <= 0)
            {
                throw new ArgumentException("Invalid ProductId", nameof(productWebsite.ProductId));
            }

            if (productWebsite.WebsiteId <= 0)
            {
                throw new ArgumentException("Invalid WebsiteId", nameof(productWebsite.WebsiteId));
            }

            String sql = "UPDATE product_websites SET WebsiteProductId = @WebsiteProductId " +
                         "WHERE ProductId = @ProductId AND WebsiteId = @WebsiteId";

            await this.dbConnection.ExecuteAsync(sql, productWebsite);

            return productWebsite;
        }

        public async Task<Int32> DeleteAsync(Int32 productId, Int32 websiteId)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Invalid ProductId", nameof(productId));
            }

            if (websiteId <= 0)
            {
                throw new ArgumentException("Invalid WebsiteId", nameof(websiteId));
            }

            String sql = "DELETE FROM product_websites WHERE ProductId = @ProductId AND WebsiteId = @WebsiteId";

            return await this.dbConnection.ExecuteAsync(sql, new { ProductId = productId, WebsiteId = websiteId });
        }
    }
}
