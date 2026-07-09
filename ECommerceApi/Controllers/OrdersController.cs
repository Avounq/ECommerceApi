using AutoMapper;
using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOrderService _orderService;

        public OrdersController(AppDbContext context, IOrderService orderService)
        {
            _context = context;
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
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
    .Select(o => new OrderResponseDto
    {
        Id = o.Id,
        CustomerId = o.CustomerId,
        CustomerName = o.Customer == null
    ? string.Empty
    : o.Customer.FirstName + " " + o.Customer.LastName,
        ProductId = o.ProductId,
        ProductName = o.Product!.Name,
        ProductPrice = o.Product.Price * (1 - o.Product.DiscountRate / 100),
        Quantity = o.Quantity,
        Status = o.Status,
        StatusUpdatedAt = o.StatusUpdatedAt,
        ShippingAddress = o.ShippingAddress,
        PaymentMethod = o.PaymentMethod,
        CardLastFourDigits = o.CardLastFourDigits
    })
    .ToList();

            var response = new PagedResponse<OrderResponseDto>
            {
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                Data = orders
            };
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddOrder([FromForm] CreateOrderDto dto)
        {
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                StatusUpdatedAt = DateTime.UtcNow


            };
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            var createdOrder = await _context.Orders
    .Where(o => o.Id == order.Id)
    .Select(o => new OrderResponseDto
    {
        Id = o.Id,
        CustomerId = o.CustomerId,
        CustomerName = o.Customer == null
    ? string.Empty
    : o.Customer.FirstName + " " + o.Customer.LastName,
        ProductId = o.ProductId,
        ProductName = o.Product!.Name,
        ProductPrice = o.Product.Price * (1 - o.Product.DiscountRate / 100),
        Quantity = o.Quantity,
        Status = o.Status,
        StatusUpdatedAt = o.StatusUpdatedAt,
        ShippingAddress = o.ShippingAddress,
        PaymentMethod = o.PaymentMethod,
        CardLastFourDigits = o.CardLastFourDigits
    })
    .FirstAsync();

            return Ok(createdOrder);
        }
        [Authorize(Roles = "Admin")]
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
            var newStatus = updatedOrder.Status.Trim();

            if (!string.Equals(order.Status, newStatus, StringComparison.Ordinal))
            {
                order.Status = newStatus;
                order.StatusUpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok("Sipariş başarıyla güncellendi.");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer == null
    ? string.Empty
    : o.Customer.FirstName + " " + o.Customer.LastName,
                    ProductId = o.ProductId,
                    ProductName = o.Product!.Name,
                    ProductPrice = o.Product.Price * (1 - o.Product.DiscountRate / 100),
                    Quantity = o.Quantity,
                    Status = o.Status,
                    StatusUpdatedAt = o.StatusUpdatedAt,
                    ShippingAddress = o.ShippingAddress,
                    PaymentMethod = o.PaymentMethod,
                    CardLastFourDigits = o.CardLastFourDigits
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new NotFoundException("Sipariş bulunamadı.");
            }

            return Ok(order);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sipariş bulunmamaktadır.");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Bu id'ye sahip sipariş başarıyla silinmiştir.");
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz."
                });
            }

            var user = await _context.Users.FindAsync(userId);

            if (user is null)
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bulunamadı."
                });
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == user.Email);

            if (customer is null)
            {
                customer = new Customer
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var basketItems = await _context.Baskets
                .Include(b => b.Product)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            if (!basketItems.Any())
            {
                return BadRequest(new
                {
                    message = "Sepet boş."
                });
            }

            var insufficientStockItem = basketItems
                .GroupBy(b => b.ProductId)
                .FirstOrDefault(group =>
                {
                    var product = group.First().Product;
                    var totalQuantity = group.Sum(b => b.Quantity);

                    return product is null || totalQuantity > product.Stock;
                });

            if (insufficientStockItem is not null)
            {
                return BadRequest(new
                {
                    message = "Sepette stokta olmayan veya yetersiz stoklu ürün var."
                });
            }

            var orders = basketItems.Select(b => new Order
            {
                CustomerId = customer.Id,
                UserId = userId,
                ProductId = b.ProductId,
                Quantity = b.Quantity,
                Status = OrderStatuses.Preparing,
                StatusUpdatedAt = DateTime.UtcNow,
                ShippingAddress = dto.Address.Trim(),
                PaymentMethod = dto.PaymentMethod.Trim(),
                CardLastFourDigits = dto.CardLastFourDigits
            }).ToList();

            _context.Orders.AddRange(orders);

            foreach (var basketItem in basketItems)
            {
                basketItem.Product!.Stock -= basketItem.Quantity;
            }

            _context.Baskets.RemoveRange(basketItems);

            await _context.SaveChangesAsync();

            var orderIds = orders.Select(o => o.Id).ToList();

            var response = await _context.Orders
                .Where(o => orderIds.Contains(o.Id))
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer == null
                        ? string.Empty
                        : o.Customer.FirstName + " " + o.Customer.LastName,
                    ProductId = o.ProductId,
                    ProductName = o.Product!.Name,
                    ProductPrice = o.Product.Price * (1 - o.Product.DiscountRate / 100),
                    Quantity = o.Quantity,
                    Status = o.Status,
                    StatusUpdatedAt = o.StatusUpdatedAt,
                    ShippingAddress = o.ShippingAddress,
                    PaymentMethod = o.PaymentMethod,
                    CardLastFourDigits = o.CardLastFourDigits
                })
                .ToListAsync();

            return Ok(response);
        }
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz."
                });
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer != null
    ? o.Customer.FirstName + " " + o.Customer.LastName
    : o.User != null
        ? o.User.FirstName + " " + o.User.LastName
        : string.Empty,
                    ProductId = o.ProductId,
                    ProductName = o.Product!.Name,
                    ProductPrice = o.Product.Price * (1 - o.Product.DiscountRate / 100),
                    Quantity = o.Quantity,
                    Status = o.Status,
                    StatusUpdatedAt = o.StatusUpdatedAt,
                    ShippingAddress = o.ShippingAddress,
                    PaymentMethod = o.PaymentMethod,
                    CardLastFourDigits = o.CardLastFourDigits
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelMyOrder(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz."
                });
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order is null)
            {
                throw new NotFoundException("Sipariş bulunamadı.");
            }

            if (order.Status == OrderStatuses.Delivered)
            {
                return BadRequest(new
                {
                    message = "Teslim edilen sipariş iptal edilemez."
                });
            }

            if (order.Status == OrderStatuses.Cancelled)
            {
                return BadRequest(new
                {
                    message = "Sipariş zaten iptal edilmiş."
                });
            }

            order.Status = OrderStatuses.Cancelled;
            order.StatusUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Sipariş iptal edildi."
            });
        }
    }
}
