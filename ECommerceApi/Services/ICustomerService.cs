using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDto>> GetAllAsync();
        Task<Customer> AddAsync(CreateCustomerDto dto);
    }
}