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
    /// <summary>
    /// Ürün İşlemlerini yönetir.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        /// <summary>
        /// ProductsController oluşturur.
        /// </summary>
        /// <param name="context">Veritabanı bağlantısı.</param>

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryParameters parameters)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.ToLower();

                query = query.Where(p => p.Name.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var response = new PagedResponse<Product>
            {
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                Data = products
            };

            return Ok(response);
        }
        /// <summary>
        /// Id'ye göre ürün getirir.
        /// </summary>
        /// <param name="id">Ürün id'si.</param>
        /// <returns>Ürün bilgisi.</returns>

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
        /// <summary>
        /// Yeni ürün ekler.
        /// </summary>
        /// <param name="dto">Eklenecek ürün bilgileri.</param>
        /// <returns>Eklenen ürün.</returns>

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDto dto)
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
        /// <summary>
        /// Ürünü günceller.
        /// </summary>
        /// <param name="id">Ürün id'si.</param>
        /// <param name="updatedProduct">Güncellenecek bilgiler.</param>
        /// <returns>İşlem sonucu.</returns>

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updatedProduct)
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
        /// <summary>
        /// Ürünü siler.
        /// </summary>B
        /// <param name="id">Ürün id'si.</param>
        /// <returns>İşlem sonucu.</returns>
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