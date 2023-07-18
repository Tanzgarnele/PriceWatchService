using System;

namespace DataAccessLayer.Models
{
    public class User
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Role { get; set; }
        public DateTime CreationDate { get; set; }
    }
}