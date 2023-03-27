using System;

namespace DataAccessLayer.Models
{
    public class Alarm
    {
        public Int32 Id { get; set; }
        public String Url { get; set; }
        public String Alias { get; set; }
        public Single Price { get; set; }
        public Int32 UserId { get; set; }
        public DateTime? EntryDate { get; set; }
        public Single? LastPrice { get; set; }
        public Single? CurrentPrice { get; set; }
        public String ThumbnailUrl { get; set; }
    }
}