namespace ECommerceApi.Dtos
{
    public class BasketResponseDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }
    }
}