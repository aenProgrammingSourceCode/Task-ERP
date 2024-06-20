using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Mapping.Automappers
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, CategoryView>().ReverseMap();
            CreateMap<Product, ProductView>().ReverseMap();
            CreateMap<Unit, ProductView>().ReverseMap();

            CreateMap<Customer, ProductView>().ReverseMap();
            CreateMap<OrderItem, OrderItemView>().ReverseMap();

            CreateMap<SalesOrderView, SalesOrder>()
                .ForMember(dst => dst.Customer, src => src.MapFrom(src => new Customer { Name = src.CustomerName }));

            CreateMap<DeliveryOrder, DeliveryOrderView>()
         .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.SalesOrder.Customer.Name))
         .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
         .ForMember(dest => dest.DeliveryOrderItems, opt => opt.MapFrom(src => src.DeliveryOrderItem.Select(doi => new DeliveryOrderItemView
         {
             Id = doi.Id,
             ProductName = doi.OrderItem.Product.Name, // Ensure ProductName is populated
             Price = doi.OrderItem.Price, // Ensure Price is populated
             DeliveryQuantity = doi.DeliveryQuantity,
             DeliveryAmount = doi.DeliveryAmount,
         })))
         .ReverseMap();

            CreateMap<DeliveryOrderItem, DeliveryOrderItemView>().ReverseMap();

            CreateMap<Customer, CustomerView>()
            .ReverseMap();
        }
    }
}
