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
        public List<Order> Orders { get; set ;} = new (); //Product'ın bir çok Order'ı olabilir
        public List<Basket> Baskets { get; set; } = new ();
}
}