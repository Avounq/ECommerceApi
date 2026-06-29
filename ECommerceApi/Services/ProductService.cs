using ECommerceApi.Repositories;
using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();

            return _mapper.Map<List<ProductResponseDto>>(products);
        }

        public async Task<Product> AddAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return product;
        }
    }
}