using ECommerceApi.Models;
using ECommerceApi.Repositories;

namespace ECommerceApi.UnitOfWork
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> Products { get;  }
        IGenericRepository<Customer> Customers { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<Basket> Baskets {  get;  }
        Task SaveChangesAsync();
    }
}
