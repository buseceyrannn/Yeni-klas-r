namespace BUSEB.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // Foreign Key (Order ilişkisi)
        public int OrderId { get; set; }

        public Order Order { get; set; }

        // Ürün bilgileri (snapshot olarak tutulur)
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}