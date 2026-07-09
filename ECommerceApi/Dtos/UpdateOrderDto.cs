using ECommerceApi.Services;

namespace ECommerceApi.Dtos
{
    public class UpdateOrderDto
    {
        public int? CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = OrderStatuses.Preparing;
    }
}
