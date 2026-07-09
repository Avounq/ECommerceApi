using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductResponseDto>();

            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();

            CreateMap<CreateBasketDto, Basket>();
            CreateMap<Basket, BasketResponseDto>();

            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer!.FirstName + " " + src.Customer.LastName))
                .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product!.Name))
                .ForMember(dest => dest.ProductPrice,
                opt => opt.MapFrom(src => src.Product!.Price * (1 - src.Product.DiscountRate / 100)));
        }
    }
}
