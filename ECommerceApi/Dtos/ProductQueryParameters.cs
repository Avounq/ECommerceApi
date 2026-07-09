namespace ECommerceApi.Dtos
{
    public class ProductQueryParameters : PaginationParameters
    {
        public string? Search { get; set; }
        public string? Category { get; set; }
        public string? SortBy { get; set; }
        public bool Descending { get; set; } = false;
        public bool InStockOnly { get; set; } = false;
        public bool LowStockOnly { get; set; } = false;
    }
}
