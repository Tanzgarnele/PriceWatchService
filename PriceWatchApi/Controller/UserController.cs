using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace PriceWatchApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dictionary<String, Object>>>> GetAllUsers()
        {
            try
            {
                IEnumerable<Dictionary<String, Object>> users = await userRepository.GetAllAsync();
                return this.Ok(users);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Dictionary<String, Object>>> GetUserById(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            try
            {
                Dictionary<String, Object> user = await userRepository.GetByIdAsync(id);

                if (user == null)
                {
                    return this.NotFound();
                }

                return this.Ok(user);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Int32>> AddUser(User user)
        {
            try
            {
                return this.CreatedAtAction(nameof(GetUserById), new { id = await userRepository.AddAsync(user) }, user);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Int32 id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            try
            {
                await userRepository.DeleteAsync(id);
                return this.NoContent();
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("{id}/changepassword")]
        public async Task<ActionResult> ChangePassword(Int32 id, String oldPassword, String newPassword)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid id.");
            }

            if (String.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException($"'{nameof(oldPassword)}' cannot be null or empty.", nameof(oldPassword));
            }

            if (String.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException($"'{nameof(newPassword)}' cannot be null or empty.", nameof(newPassword));
            }

            try
            {
                await userRepository.ChangePasswordAsync(id, oldPassword, newPassword);
                return this.NoContent();
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}