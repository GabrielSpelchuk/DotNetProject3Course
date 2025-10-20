using AutoMapper;
using Project.BLL.Dtos.Customers;
using Project.BLL.Dtos.Orders;
using Project.BLL.Dtos.Products;
using Project.BLL.Dtos.Suppliers;
using Project.Domain.Entities;

namespace Project.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //------------------------------------------
            // 🔹 ORDER
            //------------------------------------------
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0m));

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CreateOrderItemDto, OrderItem>();

            CreateMap<UpdateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());

            CreateMap<UpdateOrderItemDto, OrderItem>();

            //------------------------------------------
            // 🔹 CUSTOMER
            //------------------------------------------
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));

            //------------------------------------------
            // 🔹 PRODUCT
            //------------------------------------------
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.SupplierName,
                    opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty));

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));

            //------------------------------------------
            // 🔹 SUPPLIER
            //------------------------------------------
            CreateMap<Supplier, SupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>()
                .ForAllMembers(opt => opt.Condition((src, dest, val) => val != null));
        }
    }
}
