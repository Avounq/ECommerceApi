using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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
        public IActionResult GetAll([FromQuery] OrderQueryParameters parameters)
        {
            var query = _context.Orders.AsQueryable();

            if (parameters.CustomerId is not null)
            {
                query = query.Where(o => o.CustomerId == parameters.CustomerId);
            }

            if (parameters.ProductId is not null)
            {
                query = query.Where(o => o.ProductId == parameters.ProductId);
            }

            if (parameters.Quantity is not null)
            {
                query = query.Where(o => o.Quantity == parameters.Quantity);
            }

            var totalCount = query.Count();

            var orders = query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var response = new PagedResponse<Order>
            {
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                Data = orders
            };

            return Ok(response);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOrder([FromForm] CreateOrderDto dto)
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateOrder(int id, [FromForm] UpdateOrderDto updatedOrder)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sipariş bulunamadı.");
            }

            order.CustomerId = updatedOrder.CustomerId;
            order.ProductId = updatedOrder.ProductId;
            order.Quantity = updatedOrder.Quantity;

            await _context.SaveChangesAsync();

            return Ok("Sipariş başarıyla güncellendi.");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new NotFoundException("Sipariş bulunamadı.");
            }

            return Ok(order);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sipariş bulunmamaktadır.");

            }
            _context.Orders.Remove(order!);
            await _context.SaveChangesAsync();
            return Ok("Bu id'ye sahip sipariş başarıyla silinmişir.");
        }
}
}
