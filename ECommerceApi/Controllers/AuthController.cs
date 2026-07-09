using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerceApi.Services;


namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]




    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        public AuthController(
    IConfiguration configuration,
    AppDbContext context,
    ITokenService tokenService)
        {
            _configuration = configuration;
            _context = context;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
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

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var username = dto.Username.Trim().ToLowerInvariant();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user is null)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }
            var passwordResult = _passwordHasher.VerifyHashedPassword(
    user,
    user.PasswordHash,
    dto.Password
);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Kullanıcı adı veya şifre hatalı.");
            }
            var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, user.Role)
};
            var jwtSettings = _configuration.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpireMinutes"])
                ),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            var tokenResponse = _tokenService.CreateToken(user);

            user.RefreshToken = tokenResponse.RefreshToken;
            user.RefreshTokenExpiryTime = tokenResponse.RefreshTokenExpiration;

            await _context.SaveChangesAsync();

            return Ok(tokenResponse);
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var username = dto.Username.Trim().ToLowerInvariant();
            var email = dto.Email.Trim().ToLowerInvariant();
            var usernameExists = await _context.Users
                .AnyAsync(x => x.Username == username);
            if (usernameExists)
            {
                return Conflict(new
                {
                    message = "Bu Kullanıcı adı zaten kullanılıyor."
                });
            }
            var emailExists = await _context.Users
                .AnyAsync (x => x.Email == email);

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "Bu Email zaten kullanııyo. "
                });
            }
            var user = new User
            {
                Username = username,
                Email = email,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                dto.Password
                );

            _context.Users.Add  ( user );
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role,
                user.CreatedAt

            });
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        [Consumes("application/json")]
        public async Task<ActionResult<TokenResponseDto>> Refresh(
    [FromBody] RefreshTokenDto dto)
        {
            ClaimsPrincipal principal;

            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(
                    dto.AccessToken);
            }
            catch
            {
                return Unauthorized(new
                {
                    message = "Geçersiz access token."
                });
            }

            var userIdValue = principal.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz."
                });
            }

            var user = await _context.Users.FindAsync(userId);

            if (user is null)
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bulunamadı."
                });
            }

            if (user.RefreshToken != dto.RefreshToken)
            {
                return Unauthorized(new
                {
                    message = "Refresh token geçersiz."
                });
            }

            if (user.RefreshTokenExpiryTime is null ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new
                {
                    message = "Refresh token süresi dolmuş."
                });
            }

            var tokenResponse = _tokenService.CreateToken(user);

            user.RefreshToken = tokenResponse.RefreshToken;
            user.RefreshTokenExpiryTime =
                tokenResponse.RefreshTokenExpiration;

            await _context.SaveChangesAsync();

            return Ok(tokenResponse);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdValue = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if(!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgileri geçersiz. "
                });
            }
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return NotFound(new
                {
                    message = "Kullanıcı bulunamadı. "
                });
            }
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Başarıyla çıkış yaptınız. "
            });

        }
        [Authorize]
        [HttpGet("me")]

        public async Task<IActionResult> Me()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = " Kullanıcı bilgisi geçersiz. "
                });
            }
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userId);
            if(user is null)
            {
                return NotFound(new
                {
                    message = "Kullanıcı bulunamadı"
                });
            }
            return Ok(new
            {
             user.Id,
             user.Username,
             user.Email,
             user.FirstName,
             user.LastName,
             user.Role,
             user.CreatedAt
            });
        }

        [HttpPut("updateprofile")]
        public async Task<IActionResult> ProfileUpdate([FromBody] ProfileUpdateDto dto)
        { //Tokenden user id al.
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz."
                });
            }

            //DB den kullanıcıyı bul
            var user = await _context.Users.FirstAsync(x => x.Id == userId);

            if (user is null)
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bulunamadı"
                });
            }
            // gelen email başka kullanıcıda var mı?
            var email = dto.Email.Trim().ToLowerInvariant();
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == email && x.Id != userId);
            if(emailExists)
            {
                return Conflict(new
                {
                    message = "Bu Email başka bir kullnıcı tarafından kullanılıyor. "
                });
            }

            // izin verilen alanlar

            user.Email = email;
            user.FirstName = dto.FirstName.Trim();
            user.LastName = dto.LastName.Trim();

            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role,
                user.CreatedAt
            });
        }

        [Authorize]
        [HttpPut("sifre-degistir")]
        public async Task<IActionResult> changePassword([FromBody]ChangePasswordDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz. "
                });



            }
            var user = await _context.Users.FindAsync(userId);
            if (user is null)
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bulunamadı. "
                });
            }

            var passwordResault = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.CurrentPassword
                );
            if(passwordResault == PasswordVerificationResult.Failed)
            {
                return BadRequest(new
                {
                    message = "Hatalı Şifre girdiniz. "
                });
            }
            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                dto.NewPassword
                );
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Şifre Başarıyla Güncellendi. "
            });
        }


    }



}
