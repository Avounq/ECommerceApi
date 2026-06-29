namespace ECommerceApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Customer? Customer { get; set; } //Order'ın bir Customeri vardır
        public Product? Product { get; set; } // Order'ın bir Product'ı vardır
    }
}
