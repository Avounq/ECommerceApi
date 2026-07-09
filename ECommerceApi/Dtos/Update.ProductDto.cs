using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Dtos
{
    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Ürün adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 1000000, ErrorMessage = "Ürün fiyatı 0'dan büyük olmak zorundadır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ürün kategorisi zorunludur.")]
        [StringLength(50, ErrorMessage = "Ürün kategorisi en fazla 50 karakter olabilir.")]
        public string Category { get; set; } = "Elektronik";

        [StringLength(2000, ErrorMessage = "Görsel URL en fazla 2000 karakter olabilir.")]
        public string ImageUrl { get; set; } = string.Empty;

        public List<string> ImageUrls { get; set; } = new();

        [Range(0, 1000000, ErrorMessage = "Stok 0 veya daha büyük olmak zorundadır.")]
        public int Stock { get; set; } = 50;

        [Range(0, 90, ErrorMessage = "İndirim oranı 0 ile 90 arasında olmalıdır.")]
        public decimal DiscountRate { get; set; }
    }
}
