using System;

namespace DataAccessLayer.Models
{
    public class ProductWebsite
    {
        public Int32 Id { get; set; }
        public Int32 ProductId { get; set; }
        public Int32 WebsiteId { get; set; }
        public String WebsiteProductId { get; set; }
    }
}