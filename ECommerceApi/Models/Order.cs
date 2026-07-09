using ECommerceApi.Services;

namespace ECommerceApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public int? UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; } = OrderStatuses.Preparing;

        public DateTime StatusUpdatedAt { get; set; } = DateTime.UtcNow;

        public string ShippingAddress { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public string? CardLastFourDigits { get; set; }

        public Customer? Customer { get; set; }

        public User? User { get; set; }

        public Product? Product { get; set; }
    }
}
