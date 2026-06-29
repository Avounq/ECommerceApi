using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(AppDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();

            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(CreateOrderDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity


            };
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return Ok(order);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order updatedOrder)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Bu id'ye sahip bir sipariş bulunamadı. ");
        }
            order.CustomerId = updatedOrder.CustomerId;
            order.ProductId = updatedOrder.ProductId;
            order.Quantity = updatedOrder.Quantity;
            await _context.SaveChangesAsync();
            return Ok("Sipariş başarıyla güncellendi. ");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound("Sipariş bulunamadı.");
            }

            return Ok(order);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Bu id'ye sahip bir sipariş bulunmamaktadır. ");

            }
            _context.Orders.Remove(order!);
            await _context.SaveChangesAsync();
            return Ok("Bu id'ye sahip sipariş başarıyla silinmişir.");
        }
}
}
