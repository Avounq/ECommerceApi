namespace ECommerceApi.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int Stock { get; set; }

        public decimal DiscountRate { get; set; }

        public List<string> ImageUrls { get; set; } = new();
    }
}
