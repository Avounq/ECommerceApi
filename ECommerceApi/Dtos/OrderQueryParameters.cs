namespace ECommerceApi.Dtos
{
    public class OrderQueryParameters : PaginationParameters
    {
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
