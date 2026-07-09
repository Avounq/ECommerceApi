using ECommerceApi.Dtos;
using ECommerceApi.Models;
using System.Security.Claims;


namespace ECommerceApi.Services
{
    public interface ITokenService
    {
        TokenResponseDto CreateToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(
            string accessToken);
    }
}
