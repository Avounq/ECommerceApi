using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ECommerceApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenResponseDto CreateToken(User user)
        {
            var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
            var issuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
            var audience = Environment.GetEnvironmentVariable("Jwt__Audience");

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("Jwt__Key container içinde okunamadı.");

            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("Jwt__Issuer container içinde okunamadı.");

            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("Jwt__Audience container içinde okunamadı.");

            var accessTokenMinutes = int.TryParse(
                _configuration["Jwt:AccessTokenExpirationMinutes"],
                out var configuredAccessTokenMinutes)
                ? configuredAccessTokenMinutes
                : 15;

            var refreshTokenDays = int.TryParse(
                _configuration["Jwt:RefreshTokenExpirationDays"],
                out var configuredRefreshTokenDays)
                ? configuredRefreshTokenDays
                : 7;

            var accessTokenExpiration =
                DateTime.UtcNow.AddMinutes(accessTokenMinutes);

            var refreshTokenExpiration =
                DateTime.UtcNow.AddDays(refreshTokenDays);

            var claims = new List<Claim>
            {
                new(
                    ClaimTypes.NameIdentifier,
                    user.Id.ToString()),

                new(
                    ClaimTypes.Name,
                    user.Username),

                new(
                    ClaimTypes.Email,
                    user.Email),

                new(
                    ClaimTypes.Role,
                    user.Role),

                new(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var signingCreadentails = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: accessTokenExpiration,
                signingCredentials: signingCreadentails);

            var accessToken =
                new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration,
            };
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(
            string accessToken)
        {
            var jwtKey = GetRequiredJwtSetting("Key");
            var issuer = GetRequiredJwtSetting("Issuer");
            var audience = GetRequiredJwtSetting("Audience");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)),

                ValidateLifetime = false,

                ClockSkew = TimeSpan.Zero,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(
                accessToken,
                tokenValidationParameters,
                out var validatedToken);
            if(validatedToken is not JwtSecurityToken jwtSecurityToken)
            {
                throw new SecurityTokenException(
                    "Geçersiz Erişim Jetonu. ");
            }

            var isValidAlgorithm = jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.OrdinalIgnoreCase);


            if (!isValidAlgorithm)
            {
                throw new SecurityTokenException(
                    "Geçersiz Jeton Algoritması. ");
            }

            return principal;
        }

        private string GetRequiredJwtSetting(string settingName)
        {
            var value =
                _configuration[$"Jwt:{settingName}"]
                ?? Environment.GetEnvironmentVariable($"Jwt__{settingName}");

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Jwt:{settingName} ayarı bulunamadı.");
            }

            return value;
        }

    }

}
