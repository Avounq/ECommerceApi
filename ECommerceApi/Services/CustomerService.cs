using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomerService(IGenericRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();    

            return _mapper.Map<List<CustomerResponseDto>>(customers);
        }

        public async Task<Customer> AddAsync(CreateCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);

            await _repository.AddAsync(customer);

            await _repository.SaveChangesAsync();

            return customer;
        }
    }
}