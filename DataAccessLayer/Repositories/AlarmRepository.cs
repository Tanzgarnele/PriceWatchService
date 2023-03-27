using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class AlarmRepository : IAlarmRepository
    {
        private readonly IDbConnection dbConnection;

        public AlarmRepository(IDbConnection db)
        {
            dbConnection = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<IEnumerable<Alarm>> GetAllAsync()
        {
            String sql = "SELECT * FROM urls";
            return await this.dbConnection.QueryAsync<Alarm>(sql);
        }

        public async Task<Alarm> GetByIdAsync(Int32 id)
        {
            String sql = "SELECT * FROM urls WHERE Id = @Id";
            return await this.dbConnection.QueryFirstOrDefaultAsync<Alarm>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Alarm>> GetByUserIdAsync(Int32 userId)
        {
            String sql = "SELECT * FROM urls WHERE UserId = @UserId";
            return await this.dbConnection.QueryAsync<Alarm>(sql, new { UserId = userId });
        }

        public async Task AddAsync(Alarm alarm)
        {
            String sql = "INSERT INTO urls (Url, Alias, Price, UserId, EntryDate, LastPrice, CurrentPrice, ThumbnailUrl) " +
                      "VALUES (@Url, @Alias, @Price, @UserId, @EntryDate, @LastPrice, @CurrentPrice, @ThumbnailUrl);" +
                      "SELECT CAST(SCOPE_IDENTITY() as int)";
            await this.dbConnection.QuerySingleAsync<Alarm>(sql, alarm);
        }

        public async Task<Alarm> UpdateAsync(Alarm alarm)
        {
            String sql = "UPDATE urls SET Url = @Url, Alias = @Alias, Price = @Price, UserId = @UserId, " +
                      "EntryDate = @EntryDate, LastPrice = @LastPrice, CurrentPrice = @CurrentPrice, ThumbnailUrl = @ThumbnailUrl " +
                      "WHERE Id = @Id";
            await this.dbConnection.ExecuteAsync(sql, alarm);

            return alarm;
        }

        public async Task DeleteAsync(Int32 id)
        {
            String sql = "DELETE FROM urls WHERE Id = @Id";
            await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}