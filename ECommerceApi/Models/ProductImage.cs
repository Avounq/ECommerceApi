using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [StringLength(2000)]
        public string ImageUrl { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        public Product? Product { get; set; }
    }
}
