using AutoMapper;
using Project.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.BLL.Dtos.CustomerDtos;
using static Project.BLL.Dtos.OrderDtos;
using static Project.BLL.Dtos.OrderItemDtos;

namespace Project.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.Items, opt => opt.MapFrom(s => s.Items));
            CreateMap<CreateOrderDto, Order>();
        }
    }
}
