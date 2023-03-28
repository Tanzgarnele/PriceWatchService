using System;

namespace DataAccessLayer.Models
{
    public class Product
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public String ThumbnailUrl { get; set; }
    }
}