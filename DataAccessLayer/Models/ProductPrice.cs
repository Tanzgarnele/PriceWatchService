using System;

namespace DataAccessLayer.Models
{
    public class ProductPrice
    {
        public Int32 Id { get; set; }
        public Int32 ProductId { get; set; }
        public Int32 WebsiteId { get; set; }
        public Decimal Price { get; set; }
        public Decimal LastPrice { get; set; }
        public Decimal CurrentPrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}