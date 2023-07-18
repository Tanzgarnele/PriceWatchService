using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection dbConnection;

        public ProductRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<Dictionary<String, Object>> GetByIdAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            String sql = "SELECT Id, Name, Url, ThumbnailUrl, Website, DateAdded FROM Products WHERE Id = @Id";

            dynamic result = await this.dbConnection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

            if (result == null)
            {
                return null; // No product found with the given id
            }

            Dictionary<String, Object> product = new Dictionary<String, Object>
            {
                { "Id", result.Id },
                { "Name", result.Name },
                { "Url", result.Url },
                { "ThumbnailUrl", result.ThumbnailUrl },
                { "Website", result.Website },
                { "DateAdded", result.DateAdded }
            };

            return product;
        }

        public async Task<IEnumerable<Dictionary<String, Object>>> GetAllAsync()
        {
            String sql = "SELECT * FROM Products";

            IEnumerable<dynamic> results = await this.dbConnection.QueryAsync<dynamic>(sql);

            List<Dictionary<String, Object>> products = new List<Dictionary<String, Object>>();

            foreach (dynamic result in results)
            {
                Dictionary<String, Object> product = new Dictionary<String, Object>
                {
                    { "Id", result.Id },
                    { "Name", result.Name },
                    { "Url", result.Url },
                    { "ThumbnailUrl", result.ThumbnailUrl },
                    { "Website", result.Website },
                    { "DateAdded", result.DateAdded }
                };

                products.Add(product);
            }

            return products;
        }

        public async Task<Int32> AddAsync(Product product)
        {
            String sql = @"INSERT INTO Products (Name, Url, ThumbnailUrl, Website, DateAdded)
				   VALUES (@Name, @Url, @ThumbnailUrl, @Website, @DateAdded);
				   SELECT SCOPE_IDENTITY();";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>
            {
                { "@Name", product.Name },
                { "@Url", product.Url },
                { "@ThumbnailUrl", product.ThumbnailUrl },
                { "@Website", product.Website },
                { "@DateAdded", DateTime.Now }
            };

            return await this.dbConnection.QueryFirstOrDefaultAsync<Int32>(sql, parameters);
        }

        public async Task<Product> UpdateAsync(Int32 id, Product product)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            String sql = "UPDATE Products SET Name = @Name, Url = @Url, ThumbnailUrl = @ThumbnailUrl, Website = @Website WHERE Id = @Id";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>
            {
                { "@Name", product.Name },
                { "@Url", product.Url },
                { "@ThumbnailUrl", product.ThumbnailUrl },
                { "@Website", product.Website },
                { "@Id", id }
            };

            await this.dbConnection.ExecuteAsync(sql, parameters);

            return product;
        }

        public async Task<Int32> DeleteAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            String sql = "DELETE FROM Products WHERE Id = @Id";

            return await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}