using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BasketService(AppDbContext context, IMapper mapper)
        {
            _context = context;
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

            _context.Baskets.Add(basket);
            await _context.SaveChangesAsync();

            return basket;
        }
    }
}