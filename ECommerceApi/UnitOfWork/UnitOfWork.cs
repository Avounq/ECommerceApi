using ECommerceApi.Controllers;
using ECommerceApi.Data;
using ECommerceApi.Models;
using ECommerceApi.Repositories;

namespace ECommerceApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Customers = new GenericRepository<Customer>(_context);
            Orders = new GenericRepository<Order>(_context);
            Baskets = new GenericRepository<Basket>(_context);
        }

        public IGenericRepository<Product> Products{ get; }
        public IGenericRepository<Customer> Customers { get; }
        public IGenericRepository<Order> Orders { get; }
        public IGenericRepository<Basket> Baskets { get; }

    public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
}
}
