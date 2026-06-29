using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public interface IOrderService
    {
        Task<List<OrderResponseDto>> GetAllAsync();
        Task<Order> AddAsync(CreateOrderDto dto);
    }
}