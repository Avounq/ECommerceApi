using ECommerceApi.Dtos;
using ECommerceApi.Data;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet] //GetAll
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }
        [HttpGet("{id}")] //GetById
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Ürün bulunamadı");

            }
            return Ok(product);
        }

        [HttpPost] //AddProduct
        public async Task<IActionResult> AddProduct(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            // Not:
            // PostgreSQL Identity yapısına tercihen yapılmıştır.
            // Eğer iş gereksinimi değişirse aşağıdaki örnek kod ile
            // rastgele Id üretilebilir (teorik olarak pratik olarak test edilmedi)
            /* Eğer Id'nin ototmatik artması yerine rastgele oluşturulması istenirse aşağıdaki yöntem buna örnek gösterilebilir
             * 
             * Random random = new Random();
             * 
             * while (true)
             * {
             *     int randomId = random.Next(10000, 99999);
             *     
             *     if (!exists)
             *     {
             *         product.Id = randomId;
             *         break;
             *     }
             *  }
             *  */


            return Ok(product);
        }

        [HttpPut("{id}")] //PUT id
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Girdiğiniz ürün bulunamadı");
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            await _context.SaveChangesAsync();

            return Ok("Ürün başarıyla güncellendi");
        }

        [HttpDelete("{id}")] //DELETE
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound("Ürün bulunamadı");
            }

            _context.Products.Remove(product!);

            await _context.SaveChangesAsync();

            return Ok("Ürün başarıyla silindi");
        }
        

        
    }
}