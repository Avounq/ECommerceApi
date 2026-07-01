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
    [Authorize(Roles = "Admin")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ICustomerService _customerService;

        public CustomersController(AppDbContext context, ICustomerService customerService)
        {
            _context = context;
            _customerService = customerService;
        }
        [HttpGet]
        [HttpGet]
        public IActionResult GetAll([FromQuery] CustomerQueryParameters parameters)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.ToLower();

                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    c.Email.ToLower().Contains(search));
            }

            var totalCount = query.Count();

            var customers = query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var response = new PagedResponse<Customer>
            {
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)parameters.PageSize),
                Data = customers
            };

            return Ok(response);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddCustomer([FromForm] CreateCustomerDto dto)
        {
            return Ok(await _customerService.AddAsync(dto));
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromForm] UpdateCustomerDto updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new NotFoundException("Böyle bir müşteri bulunamadı.");
            }

            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;
            customer.Email = updatedCustomer.Email;

            await _context.SaveChangesAsync();

            return Ok("Müşteri bilgileri başarıyla güncellendi");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                throw new NotFoundException("Bu bilgilere sahip bir müşteri bulunmamaktadır.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok("Bilgileri girilen müşteri başarıyla silindi.");
        }
    }
}