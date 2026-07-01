
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerceApi.Dtos
{
    public class LoginDto
    {
        [DefaultValue("admin")]
        public string Username { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [DefaultValue("123456")]
        public string Password { get; set; } = string.Empty;
    }
}