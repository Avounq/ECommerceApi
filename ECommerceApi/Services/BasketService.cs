using AutoMapper;
using ECommerceApi.Repositories;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Basket> _repository;
        private readonly IMapper _mapper;

        public BasketService(
    AppDbContext context,
    IGenericRepository<Basket> repository,
    IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }
        

        public async Task<List<BasketResponseDto>> GetAllAsync()
        {
            var baskets = await _context.Baskets
                .Include(b => b.Customer)
                .Include(b => b.Product)
                .ToListAsync();

            return _mapper.Map<List<BasketResponseDto>>(baskets);
        }

        public async Task<Basket> AddAsync(CreateBasketDto dto)
        {
            var basket = _mapper.Map<Basket>(dto);

            await _repository .AddAsync(basket);
            await _repository.SaveChangesAsync();

            return basket;
        }
    }
}