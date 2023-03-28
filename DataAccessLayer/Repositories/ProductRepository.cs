using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection dbConnection;
        private readonly IConfiguration configuration;
        private readonly List<String> allowedUrls;

        public ProductRepository(IDbConnection dbConnection, IConfiguration configuration)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this.allowedUrls = new List<String>();

            IConfigurationSection allowedUrlsSection = this.configuration.GetSection("AllowedUrls");
            allowedUrls.AddRange(allowedUrlsSection.GetChildren().Select(child => child.Value));
        }

        public async Task<Product> GetByIdAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid Id.");
            }

            String sql = "SELECT * FROM products WHERE Id = @Id";

            return await this.dbConnection.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        public async Task<Product> AddAsync(Product product)
        {
            if (String.IsNullOrWhiteSpace(product.Name) || String.IsNullOrWhiteSpace(product.Url))
            {
                throw new ArgumentException("Name and Url are required fields.");
            }

            if (!this.IsValidUrl(product.Url))
            {
                throw new ArgumentException("Invalid URL.");
            }

            String sql = "INSERT INTO products (Name, Url, ThumbnailUrl) VALUES (@Name, @Url, @ThumbnailUrl); SELECT CAST(SCOPE_IDENTITY() as int)";

            Int32 id = await this.dbConnection.QuerySingleAsync<Int32>(sql, product);
            product.Id = id;

            return product;
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            if (product.Id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(product.Id), "Invalid Id.");
            }

            if (String.IsNullOrWhiteSpace(product.Name) || String.IsNullOrWhiteSpace(product.Url))
            {
                throw new ArgumentException("Name and Url are required fields.");
            }

            if (!this.IsValidUrl(product.Url))
            {
                throw new ArgumentException("Invalid URL.");
            }

            String sql = "UPDATE products SET Name = @Name, Url = @Url, ThumbnailUrl = @ThumbnailUrl WHERE Id = @Id";

            await this.dbConnection.ExecuteAsync(sql, product);

            return product;
        }

        public async Task<Int32> DeleteAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Invalid Id.");
            }

            String sql = "DELETE FROM products WHERE Id = @Id";

            return await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        private Boolean IsValidUrl(String url)
        {
            Boolean isValidUri = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;

            if (isValidUri)
            {
                String host = uriResult.Host.ToLower();

                return allowedUrls.Any(u => host.Contains(new Uri(u).Host.ToLower()));
            }

            return false;
        }
    }
}