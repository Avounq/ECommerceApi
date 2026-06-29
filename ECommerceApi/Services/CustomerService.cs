using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CustomerService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _context.Customers.ToListAsync();

            return _mapper.Map<List<CustomerResponseDto>>(customers);
        }

        public async Task<Customer> AddAsync(CreateCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;
        }
    }
}