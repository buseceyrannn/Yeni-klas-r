using System;
using System.Collections.Generic;

namespace BUSEB.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public decimal TotalPrice { get; set; }

        // Bir siparişte birden fazla ürün olur
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}