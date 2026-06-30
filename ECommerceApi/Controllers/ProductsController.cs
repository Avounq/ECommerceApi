using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    
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
                throw new NotFoundException("Aradağınız ürün mevcut değil.");

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new NotFoundException("Girdiğiniz ürün mevcut değil.");
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
                throw new NotFoundException("Girdiğiniz ürün mevcut değil. ");
            }

            _context.Products.Remove(product!);

            await _context.SaveChangesAsync();

            return Ok("Ürün başarıyla silindi");
        }
        

        
    }
}