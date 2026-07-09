using System.ComponentModel.DataAnnotations;

 namespace ECommerceApi.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur. ")]
        [StringLength(50, MinimumLength = 6,
            ErrorMessage = "Kullanıcı adı 6 ila 50 karakter arası olmak zorundadır. ")]
        public string Username { get; set; } = string.Empty;

        //======================================================================================//


        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz. ")]
        [StringLength(50, MinimumLength = 20,
            ErrorMessage = "E-posta 20 ila 50 karakter arası olmak zorundadır. ")]
        public string Email {  get; set; } = string.Empty;

        //======================================================================================//

        [Required(ErrorMessage = "Ad bölümü boş bırakılamaz. ")]
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "Adınız 3 ila 20 karakter arasında olmak zorundadır. ")]
        public string FirstName { get; set; } = string.Empty;

        //======================================================================================//

        [Required(ErrorMessage = "Soyadı alanı boş bırakılamaz. ")]
        [StringLength(20, MinimumLength = 3,
            ErrorMessage = "Soyadınız 3 ila 20 karakter arası olmak zorundadır")]
        public string LastName { get; set; } = string.Empty;

        //======================================================================================//

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz. ")]
        [StringLength(12, MinimumLength = 8,
            ErrorMessage = "Şifreniz 8-12 karakter uzunluğunda olmalıdır")]
        public string Password {  get; set; } = string.Empty;

        //======================================================================================//

        [Required(ErrorMessage = "Girdiğiniz şifre öncekiyle uyuşmuyor.")]
        [Compare(nameof(Password), ErrorMessage = "Girilen şifre uyuşmuyor.")]


        public string ConfirmPassword {  get; set; } = string.Empty;
    }
}
