using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Dtos
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "Teslimat adresi zorunludur.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Teslimat adresi 10 ile 500 karakter arasında olmalıdır.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ödeme tipi zorunludur.")]
        [StringLength(50, ErrorMessage = "Ödeme tipi en fazla 50 karakter olabilir.")]
        public string PaymentMethod { get; set; } = "Kapıda Ödeme";

        [RegularExpression(@"^\d{4}$", ErrorMessage = "Kart son 4 hanesi 4 rakam olmalıdır.")]
        public string? CardLastFourDigits { get; set; }
    }
}
