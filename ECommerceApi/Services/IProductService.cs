using ECommerceApi.Dtos;
using ECommerceApi.Models;
namespace ECommerceApi.Services
{
    
        public interface IProductService
        {
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<Product> AddAsync(CreateProductDto dto);
        }
    
}
