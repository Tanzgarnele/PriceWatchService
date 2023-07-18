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

        public async Task<IEnumerable<Dictionary<String, Object>>> GetAllAsync()
        {
            String sql = "SELECT Id, Name, Email, Role, CreationDate FROM Users";


            IEnumerable<dynamic> results = await this.dbConnection.QueryAsync<dynamic>(sql);

            List<Dictionary<String, Object>> products = new List<Dictionary<String, Object>>();

            foreach (dynamic result in results)
            {
                Dictionary<String, Object> product = new Dictionary<String, Object>
                {
                    { "Id", result.Id },
                    { "Name", result.Name },
                    { "Email", result.Email },
                    { "Role", result.Role },
                    { "CreationDate", DateTime.Now }
                };

                products.Add(product);
            }

            return products;
        }

        public async Task<Dictionary<String, Object>> GetByIdAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            String sql = "SELECT Id, Name, Email, Password, PasswordSalt, Role, CreationDate FROM Users WHERE Id = @Id";

            dynamic result = await this.dbConnection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });

            if (result == null)
            {
                return null;
            }

            Dictionary<String, Object> product = new Dictionary<String, Object>
            {
                { "Id", result.Id },
                { "Name", result.Name },
                { "Email", result.Email },
                { "Password", result.Password },
                { "PasswordSalt", result.PasswordSalt },
                { "Role", result.Role },
                { "CreationDate", DateTime.Now }
            };

            return product;
        }

        public async Task<Int32> AddAsync(User user)
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

            Dictionary<String, Object> parameters = new Dictionary<String, Object>
            {
                { "Name", user.Name },
                { "Email", user.Email },
                { "Password", hashedPassword },
                { "PasswordSalt", salt },
                { "Role", user.Role },
                { "CreationDate", DateTime.Now }
            };

            String sql = "INSERT INTO Users (Name, Email, Password, PasswordSalt , Role, CreationDate) " +
                         "VALUES (@Name, @Email, @Password, @PasswordSalt , @Role, @CreationDate); SELECT CAST(SCOPE_IDENTITY() as int)";

            return await this.dbConnection.QuerySingleAsync<Int32>(sql, parameters);
        }

        public async Task<Int32> DeleteAsync(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            String sql = "DELETE FROM Users WHERE Id = @Id";

            return await this.dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<User> GetByEmailAsync(String email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.");
            }

            String sql = "SELECT Id, Name, Email, Password, PasswordSalt, Role, CreationDate " +
                         "FROM Users WHERE Email = @Email";

            return await this.dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
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

            Dictionary<String, Object> user = await this.GetByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException($"Invalid UserId.");
            }

            if (!this.VerifyPasswordHash(oldPassword, user["Password"].ToString(), user["PasswordSalt"].ToString()))
            {
                throw new ArgumentException("Old password is incorrect.");
            }

            String newSalt = BCryptNet.GenerateSalt();
            String newHashedPassword = BCryptNet.HashPassword(newPassword, newSalt);

            String sql = "UPDATE Users SET Password = @Password, PasswordSalt = @PasswordSalt WHERE Id = @Id";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>
            {
                { "Password", newHashedPassword },
                { "PasswordSalt", newSalt },
                { "Id", userId }
            };

            await this.dbConnection.ExecuteAsync(sql, parameters);

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