using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Dtos
{
    public class CreateProductReviewDto
    {
        [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Yorum zorunludur.")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Yorum 3 ile 1000 karakter arasında olmalıdır.")]
        public string Comment { get; set; } = string.Empty;
    }
}
