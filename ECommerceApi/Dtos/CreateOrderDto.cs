using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Dtos
{
    public class CreateOrderDto
    {
        [Range(0.01, 10000000, ErrorMessage = "Müşteri ID'si 0'dan büyük olmalıdır. ")]
        public int CustomerId { get; set; }
        [Range(0.01, 1000000, ErrorMessage = "Ürün ID'si 0'dan büyük olmalıdır. ")]
        public int ProductId { get; set; }
        [Range(0.01, 1000000, ErrorMessage = "Ürün Miktarı 0'dan büyük olmalıdır. " )]
        public int Quantity { get; set; }
    }
}
