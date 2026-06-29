using System.ComponentModel.DataAnnotations;
namespace ECommerceApi.Dtos
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Ürün adı zorunludur. ")]
        [StringLength(100, ErrorMessage = "Ürün adı en falza 100 karakterden oluşabilir.")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 1000000, ErrorMessage = "Ürün fiyatı 0'dan büyük olmak zorundadır.")]
        public decimal Price { get; set; }
    }
}
