using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductResponseDto>>(products);
        }

        public async Task<Product> AddAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }
    }
}