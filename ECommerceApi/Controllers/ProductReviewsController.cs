using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/products/{productId:int}/reviews")]
    public class ProductReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var productExists = await _context.Products.AnyAsync(product => product.Id == productId);

            if (!productExists)
            {
                throw new NotFoundException("Ürün bulunamadı.");
            }

            var reviews = await _context.ProductReviews
                .Where(review => review.ProductId == productId)
                .OrderByDescending(review => review.CreatedAt)
                .Select(review => new ProductReviewResponseDto
                {
                    Id = review.Id,
                    ProductId = review.ProductId,
                    UserId = review.UserId,
                    UserName = review.User == null
                        ? string.Empty
                        : review.User.FirstName + " " + review.User.LastName,
                    Rating = review.Rating,
                    Comment = review.Comment,
                    CreatedAt = review.CreatedAt
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProductReview(
            int productId,
            [FromBody] CreateProductReviewDto dto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Kullanıcı bilgisi geçersiz."
                });
            }

            var productExists = await _context.Products.AnyAsync(product => product.Id == productId);

            if (!productExists)
            {
                throw new NotFoundException("Ürün bulunamadı.");
            }

            var review = new ProductReview
            {
                ProductId = productId,
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

            var createdReview = await _context.ProductReviews
                .Where(currentReview => currentReview.Id == review.Id)
                .Select(currentReview => new ProductReviewResponseDto
                {
                    Id = currentReview.Id,
                    ProductId = currentReview.ProductId,
                    UserId = currentReview.UserId,
                    UserName = currentReview.User == null
                        ? string.Empty
                        : currentReview.User.FirstName + " " + currentReview.User.LastName,
                    Rating = currentReview.Rating,
                    Comment = currentReview.Comment,
                    CreatedAt = currentReview.CreatedAt
                })
                .FirstAsync();

            return StatusCode(StatusCodes.Status201Created, createdReview);
        }
    }
}
