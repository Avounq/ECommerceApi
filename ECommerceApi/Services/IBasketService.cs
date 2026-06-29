using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public interface IBasketService
    {
        Task<List<BasketResponseDto>> GetAllAsync();

        Task<Basket> AddAsync(CreateBasketDto dto);
    }
}