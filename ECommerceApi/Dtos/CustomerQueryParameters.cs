namespace ECommerceApi.Dtos
{
    public class CustomerQueryParameters : PaginationParameters
    {
        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; } = false;
    }
}