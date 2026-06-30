namespace ECommerceApi.Dtos
{
    public class UpdateOrderDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}