using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private readonly IDbConnection dbConnection;

        public WebsiteRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<Website>> GetAllAsync()
        {
            String sql = "SELECT * FROM product_websites";

            return await this.dbConnection.QueryAsync<Website>(sql);
        }

        public async Task<Website> GetByIdAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Id.");
            }

            String sql = "SELECT * FROM product_websites WHERE Id = @Id";

            return await this.dbConnection.QuerySingleOrDefaultAsync<Website>(sql, new { Id = id });
        }

        public async Task<Website> AddAsync(Website website)
        {
            if (String.IsNullOrEmpty(website.Name) || website.Name.Length < 3)
            {
                throw new ArgumentException("Website name must be at least 3 characters long.");
            }

            String sql = "INSERT INTO product_websites (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int)";

            Int32 id = await this.dbConnection.QuerySingleAsync<Int32>(sql, website);
            website.Id = id;

            return website;
        }

        public async Task<Int32> DeleteAsync(Int32 id)
        {
            String sql = "DELETE FROM product_websites WHERE Id = @Id";

            return await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<Website> UpdateAsync(Website website)
        {
            if (String.IsNullOrEmpty(website.Name) || website.Name.Length < 3)
            {
                throw new ArgumentException("Website name must be at least 3 characters long.");
            }

            String sql = "UPDATE product_websites SET Name = @Name WHERE Id = @Id";

            await this.dbConnection.ExecuteAsync(sql, website);

            return website;
        }
    }
}