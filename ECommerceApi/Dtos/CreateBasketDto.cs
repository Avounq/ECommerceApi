
using System.ComponentModel.DataAnnotations;
namespace ECommerceApi.Dtos
{
    public class CreateBasketDto
    {
        [Range(1, 10000000, ErrorMessage = "Müşteri ID'si 0'dan büyük olmalıdır. ")]
        public int CustomerId { get; set; }
        [Range(1, 1000000, ErrorMessage = "Ürün ID'si 0'dan büyük olmalıdır. ")]
        public int ProductId { get; set; }
        [Range(1, 1000000, ErrorMessage = "Ürün miktar 0'dan büyük olmalıdır. ")]
        public int Quantity { get; set; }
    }
}
