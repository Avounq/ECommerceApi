using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BasketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBasketService _basketService;

        public BasketsController(AppDbContext context, IBasketService basketService)
        {
            _context = context;
            _basketService = basketService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] BasketQueryParameters parameters)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(userIdValue, out var userID))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz. "
                });
            }
            var query = _context.Baskets
        .Where(b => b.UserId == userID)
        .AsQueryable();

            if (parameters.CustomerId is not null)
            {
                query = query.Where(b => b.CustomerId == parameters.CustomerId);
            }

            if (parameters.ProductId is not null)
            {
                query = query.Where(b => b.ProductId == parameters.ProductId);
            }

            if (parameters.Quantity is not null)
            {
                query = query.Where(b => b.Quantity == parameters.Quantity);
            }

            var totalCount = query.Count();

            var baskets = query
    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
    .Take(parameters.PageSize)
    .Select(b => new BasketResponseDto
    {
        Id = b.Id,
        ProductId = b.ProductId,
        Quantity = b.Quantity
    })
    .ToList();

            var response = new PagedResponse<BasketResponseDto>
            {
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                Data = baskets
            };

            return Ok(response);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddBasket([FromForm] CreateBasketDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {

                    message = "Kullanıcı bilgisi geçersiz"
                });
            }

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product is null)
            {
                throw new NotFoundException("Ürün bulunamadı.");
            }

            var existingBasket = await _context.Baskets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == dto.ProductId);

            var requestedQuantity = (existingBasket?.Quantity ?? 0) + dto.Quantity;

            if (requestedQuantity > product.Stock)
            {
                return BadRequest(new
                {
                    message = "Yeterli stok yok."
                });
            }

            if (existingBasket is not null)
            {
                existingBasket.Quantity = requestedQuantity;
            }
            else
            {
                existingBasket = new Basket
                {
                    UserId = userId,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                };
                _context.Baskets.Add(existingBasket);
            }

            await _context.SaveChangesAsync();

            return Ok(new BasketResponseDto
            {
                Id = existingBasket.Id,
                ProductId = existingBasket.ProductId,
                Quantity = existingBasket.Quantity
            });
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateBasket(int id, [FromForm] UpdateBasketDto updatedBasket)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgsi geçersiz."
                });
            }

            var basket = await _context.Baskets
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if(basket == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sepet bulunamadı. ");
            }

            var product = await _context.Products.FindAsync(updatedBasket.ProductId);
            if (product is null)
            {
                throw new NotFoundException("Ürün bulunamadı.");
            }

            var otherBasketQuantity = await _context.Baskets
                .Where(b =>
                    b.UserId == userId &&
                    b.ProductId == updatedBasket.ProductId &&
                    b.Id != id)
                .SumAsync(b => b.Quantity);

            if (otherBasketQuantity + updatedBasket.Quantity > product.Stock)
            {
                return BadRequest(new
                {
                    message = "Yeterli stok yok."
                });
            }

            basket.ProductId = updatedBasket.ProductId;
            basket.Quantity = updatedBasket.Quantity;

            await _context.SaveChangesAsync();
            return Ok("Sepet Başarıyla Güncellendi");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = " Kullanıcı bilgi geçersiz. "
                });

            }
            var basket = await _context.Baskets
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if(basket == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sepet bulunamadı.");
            }
            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();

            return Ok("Girilen İd'li sepet başarıyla silindi.");
    }
    }
}
