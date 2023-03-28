using System;

namespace DataAccessLayer.Models
{
    public class User
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public String PasswordHash { get; set; }
        public String PasswordSalt { get; set; }
    }
}