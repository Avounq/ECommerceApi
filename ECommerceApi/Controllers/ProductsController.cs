using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Controllers
{
    /// <summary>
    /// Ürün İşlemlerini yönetir.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    
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
            var query = _context.Products
                .Include(p => p.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.ToLower();

                query = query.Where(p => p.Name.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Category))
            {
                var category = parameters.Category.Trim().ToLower();

                query = query.Where(p => p.Category.ToLower() == category);
            }

            if (parameters.InStockOnly)
            {
                query = query.Where(p => p.Stock > 0);
            }

            if (parameters.LowStockOnly)
            {
                query = query.Where(p => p.Stock > 0 && p.Stock <= 5);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.Descending
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name),

                    "price" => parameters.Descending
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price),

                    "category" => parameters.Descending
                        ? query.OrderByDescending(p => p.Category)
                        : query.OrderBy(p => p.Category),

                    _ => query
                };
            }

            var totalCount = await query.CountAsync();

            var productEntities = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var products = productEntities
                .Select(ToResponseDto)
                .ToList();

            var response = new PagedResponse<ProductResponseDto>
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
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Aradağınız ürün mevcut değil.");

            }
            return Ok(ToResponseDto(product));
        }
        /// <summary>
        /// Yeni ürün ekler.
        /// </summary>
        /// <param name="dto">Eklenecek ürün bilgileri.</param>
        /// <returns>Eklenen ürün.</returns>

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Category = dto.Category.Trim(),
                ImageUrl = dto.ImageUrl.Trim(),
                Stock = dto.Stock,
                DiscountRate = dto.DiscountRate
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();
            SetProductImages(product, dto.ImageUrls);
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


            return Ok(ToResponseDto(product));
        }
        /// <summary>
        /// Ürünü günceller.
        /// </summary>
        /// <param name="id">Ürün id'si.</param>
        /// <param name="updatedProduct">Güncellenecek bilgiler.</param>
        /// <returns>İşlem sonucu.</returns>

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updatedProduct)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                throw new NotFoundException("Girdiğiniz ürün mevcut değil.");
            }

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Category = updatedProduct.Category.Trim();
            product.ImageUrl = updatedProduct.ImageUrl.Trim();
            product.Stock = updatedProduct.Stock;
            product.DiscountRate = updatedProduct.DiscountRate;
            SetProductImages(product, updatedProduct.ImageUrls);

            await _context.SaveChangesAsync();

            return Ok("Ürün başarıyla güncellendi");
        }
        /// <summary>
        /// Ürünü siler.
        /// </summary>
        /// <param name="id">Ürün id'si.</param>
        /// <returns>İşlem sonucu.</returns>
        [HttpDelete("{id}")] //DELETE
        [Authorize(Roles = "Admin")]
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
        

        private static ProductResponseDto ToResponseDto(Product product)
        {
            var imageUrls = product.Images
                .OrderBy(image => image.DisplayOrder)
                .Select(image => image.ImageUrl)
                .Where(imageUrl => !string.IsNullOrWhiteSpace(imageUrl))
                .ToList();

            if (!string.IsNullOrWhiteSpace(product.ImageUrl)
                && !imageUrls.Contains(product.ImageUrl))
            {
                imageUrls.Insert(0, product.ImageUrl);
            }

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                DiscountRate = product.DiscountRate,
                ImageUrls = imageUrls
            };
        }

        private static void SetProductImages(Product product, IEnumerable<string> imageUrls)
        {
            product.Images.Clear();

            var normalizedImageUrls = imageUrls
                .Where(imageUrl => !string.IsNullOrWhiteSpace(imageUrl))
                .Select(imageUrl => imageUrl.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(imageUrl => !string.Equals(imageUrl, product.ImageUrl, StringComparison.OrdinalIgnoreCase))
                .ToList();

            for (var index = 0; index < normalizedImageUrls.Count; index++)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = normalizedImageUrls[index],
                    DisplayOrder = index + 1
                });
            }
        }
    }
}
