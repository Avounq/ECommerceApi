using ECommerceApi.Data;
using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using Microsoft.EntityFrameworkCore;
namespace ECommerceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OrderService(AppDbContext context, IMapper mapper)
        {
            _context = context;
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

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
    }
}
}
