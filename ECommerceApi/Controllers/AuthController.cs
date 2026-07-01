using ECommerceApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// JWT Token üretir.
        /// </summary>
        /// <remarks>
        /// Demo Hesapları:
        ///
        /// Admin Username: admin Password: 123456
        ///
        /// User Username: user Password: 123456
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromForm] LoginDto dto)
        {
            if (dto.Username != "admin" || dto.Password != "123456")
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }
            var claims = new[]
{
    new Claim(ClaimTypes.Name, dto.Username),
    new Claim(ClaimTypes.Role, "Admin")
};
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new
            {
                token = tokenString,
            });

        }
    }
}
