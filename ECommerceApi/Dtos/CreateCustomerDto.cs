using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.Dtos
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "İsim bilgisi zorunludur ")]
        [StringLength(25, ErrorMessage = "isim en falza 18 karakterden oluşabilir.")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Soyisim bilgisi zorunludur ")]
        [StringLength(25, ErrorMessage = "Soy isim en falza 18 karakterden oluşabilir.")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email girişi zorunludur ")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz. ")]
        [StringLength(50, ErrorMessage = "Email en falza 18 karakterden oluşabilir.")]

        public string Email {  get; set; } = string.Empty;
    }
}
