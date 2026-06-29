using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Repositories;
using ECommerceApi.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BasketService(
    AppDbContext context,
    IUnitOfWork unitOfWork,
    IMapper mapper)
        {
            _context = context;
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.Baskets.AddAsync(basket);
            await _unitOfWork.SaveChangesAsync();

            return basket;
        }
    }
}