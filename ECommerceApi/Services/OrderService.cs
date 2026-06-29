using ECommerceApi.Data;
using ECommerceApi.Repositories;
using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
namespace ECommerceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Order> _repository;
        private readonly IMapper _mapper;
        public OrderService(
    AppDbContext context,
    IGenericRepository<Order> repository,
    IMapper mapper)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .ToListAsync();

            return _mapper.Map<List<OrderResponseDto>>(orders);
        }
        public async Task<Order> AddAsync(CreateOrderDto dto)
        {
            var order = _mapper.Map<Order>(dto);

            await _repository.AddAsync(order);
            await _repository.SaveChangesAsync();
            return order;
    }
}
}
