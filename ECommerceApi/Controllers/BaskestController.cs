using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAll()
        {
            var baskets = await _basketService.GetAllAsync();

            return Ok(baskets);
        }

        [HttpPost]
        public async Task<IActionResult> AddBasket(CreateBasketDto dto)
        {
            var basket = await _basketService.AddAsync(dto);

            return Ok(basket);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBasket(int id, Basket updatedBaskter)
        {
            var basket = await _context.Baskets.FindAsync(id);

            if (basket == null)
            {
                return NotFound("Bu id'ye sahip bir sepet bulunamadı. ");
        }
            basket.CustomerId = updatedBaskter.CustomerId;
            basket.ProductId = updatedBaskter.ProductId;
            basket.Quantity = updatedBaskter.Quantity;
            await _context.SaveChangesAsync();
            return Ok("Sepet başarıyla güncellendi.");
    

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var basket = await _context.Baskets.FindAsync(id);

            if ( basket == null)
            {
                return NotFound("Bu id'ye sahip bir sepet bulunamadı");
        }
            _context.Baskets.Remove(basket); 
            await _context.SaveChangesAsync();
            return Ok("Girilen id'li sepet başarıyla silindi");
}
}
}
