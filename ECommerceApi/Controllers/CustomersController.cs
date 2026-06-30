using ECommerceApi.Data;
using ECommerceApi.Dtos;
using ECommerceApi.Exceptions;
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
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _customerService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CreateCustomerDto dto)
        {
            return Ok(await _customerService.AddAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto updatedCustomer)
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