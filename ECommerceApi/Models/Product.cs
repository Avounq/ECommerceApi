using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur. ")]
        [StringLength(100, ErrorMessage = "Ürün adı en falza 100 karakterden oluşabilir. ")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01,1000000, ErrorMessage = "Ürün fiyatı 0'dan büyük olmak zorundadır. ")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = "Elektronik";

        [StringLength(2000)]
        public string ImageUrl { get; set; } = string.Empty;

        [Range(0, 1000000, ErrorMessage = "Stok 0 veya daha büyük olmak zorundadır. ")]
        public int Stock { get; set; } = 50;

        [Range(0, 90, ErrorMessage = "İndirim oranı 0 ile 90 arasında olmalıdır. ")]
        public decimal DiscountRate { get; set; }

        public List<Order> Orders { get; set ;} = new ();
        public List<Basket> Baskets { get; set; } = new ();
        public List<ProductImage> Images { get; set; } = new();
        public List<ProductReview> Reviews { get; set; } = new();
}
}
