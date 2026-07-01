using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var query = _context.Baskets.AsQueryable();

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
                .ToList();

            var response = new PagedResponse<Basket>
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
            var basket = await _basketService.AddAsync(dto);

            return Ok(basket);
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateBasket(int id, [FromForm] UpdateBasketDto updatedBasket)
        {
            var basket = await _context.Baskets.FindAsync(id);

            if (basket == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sepet bulunamadı.");
            }

            basket.CustomerId = updatedBasket.CustomerId;
            basket.ProductId = updatedBasket.ProductId;
            basket.Quantity = updatedBasket.Quantity;

            await _context.SaveChangesAsync();

            return Ok("Sepet başarıyla güncellendi.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);

            if ( basket == null)
            {
                throw new NotFoundException("Bu id'ye sahip bir sepet bulunamadı.");
            }
            _context.Baskets.Remove(basket); 
            await _context.SaveChangesAsync();
            return Ok("Girilen id'li sepet başarıyla silindi");
}
}
}
