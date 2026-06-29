using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Repositories;
using ECommerceApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();   

            return _mapper.Map<List<CustomerResponseDto>>(customers);
        }

        public async Task<Customer> AddAsync(CreateCustomerDto dto)
        {
            var customer = _mapper.Map<Customer>(dto);

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            return customer;
        }
    }
}