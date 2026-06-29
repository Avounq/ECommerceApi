namespace ECommerceApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new(); // Customer'in bir çok Order'ı olabilir
        public List<Basket> Baskets { get; set; } = new();
    }
}
