namespace ECommerceApi.Dtos
{
    public class BasketQueryParameters : PaginationParameters
    {
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
       
    }

}
