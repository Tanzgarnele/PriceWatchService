using Dapper;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            String sql = "SELECT Id, Name, Email, Role, CreatedAt FROM users";

            return await this.dbConnection.QueryAsync<User>(sql);
        }

        public async Task<User> GetByIdAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Id");
            }

            String sql = "SELECT Id, Name, Email, Role, CreatedAt FROM users WHERE Id = @Id";

            return await this.dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User> AddAsync(User user)
        {
            if (String.IsNullOrWhiteSpace(user.Name) || String.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("Name and Password are required fields.");
            }

            if (user.Password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }

            String salt = BCryptNet.GenerateSalt();
            String hashedPassword = BCryptNet.HashPassword(user.Password, salt);

            user.PasswordHash = hashedPassword;
            user.PasswordSalt = salt;

            String sql = "INSERT INTO users (Name, Email, PasswordHash, PasswordSalt, Role, CreatedAt) VALUES (@Name, @Email, @PasswordHash, @PasswordSalt, @Role, @CreatedAt); SELECT CAST(SCOPE_IDENTITY() as int)";
            Int32 id = await this.dbConnection.QuerySingleAsync<Int32>(sql, user);
            user.Id = id;

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            if (user.Id <= 0)
            {
                throw new ArgumentException("Invalid Id.");
            }

            if (String.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("Name is a required field.");
            }

            if (!String.IsNullOrEmpty(user.Password) && user.Password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }

            if (!String.IsNullOrEmpty(user.Password))
            {
                String newSalt = BCryptNet.GenerateSalt();
                String newHashedPassword = BCryptNet.HashPassword(user.Password, newSalt);

                user.Password = newHashedPassword;
                user.PasswordSalt = newSalt;
            }

            String sql = "UPDATE users SET Name = @Name, Email = @Email, Password = @Password, PasswordSalt = @PasswordSalt, Role = @Role, CreatedAt = @CreatedAt WHERE Id = @Id";
            await this.dbConnection.ExecuteAsync(sql, user);

            return user;
        }

        public async Task<Int32> DeleteAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Id.");
            }

            String sql = "DELETE FROM users WHERE Id = @Id";

            return await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<Boolean> ChangePasswordAsync(Int32 userId, String oldPassword, String newPassword)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid UserId.");
            }

            if (String.IsNullOrEmpty(oldPassword) || String.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Old password and new password are required fields.");
            }

            User user = await this.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException($"Invalid UserId.");
            }

            if (!this.VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
            {
                throw new ArgumentException("Old password is incorrect.");
            }

            String newSalt = BCryptNet.GenerateSalt();
            String newHashedPassword = BCryptNet.HashPassword(newPassword, newSalt);

            user.PasswordHash = newHashedPassword;
            user.PasswordSalt = newSalt;
            await this.UpdateAsync(user);

            return true;
        }

        private Boolean VerifyPasswordHash(String password, String storedPasswordHash, String storedSalt)
        {
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is a required field.");
            }

            String hashedPassword = BCryptNet.HashPassword(password, storedSalt);

            return hashedPassword == storedPasswordHash;
        }

    }
}