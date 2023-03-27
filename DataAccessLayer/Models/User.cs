using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class User
    {
        public Int32 Id { get; set; }
        public String Mention { get; set; }
        public String Username { get; set; }
        public DateTime? EntryDate { get; set; }
        public List<Alarm> Alarms { get; set; }
    }
}