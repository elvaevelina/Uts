using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UtsRestfullAPI.DTO;
using UtsRestfullAPI.Models;

namespace UtsRestfullAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Saless, SalessDTO>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleItems))  // <-- mapping tambahan ini
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                    src.SaleItems != null ? src.SaleItems.Sum(si => si.Quantity * si.Price) : 0
                    ))
                .ReverseMap();
            CreateMap<SaleItem, SaleItemDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<ProductAddDTO, Product>();
            CreateMap<CategoryAddDTO, Category>();
            CreateMap<CustomerAddDTO, Customer>();
            CreateMap<SalessAddDTO, Saless>();
            CreateMap<SaleItemAddDTO, SaleItem>().ReverseMap();
            CreateMap<EmployeeAddDTO, Employee>();
            CreateMap<ProductEditDTO, Product>();
            CreateMap<Saless, SalessGetDTO>().ReverseMap();
            CreateMap<SaleItem, SaleItemGetDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
            CreateMap<Member, MemberDTO>().ReverseMap();
            CreateMap<LoginDTO, Member>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<RegisterDTO, Member>().ReverseMap();

        }
    }
}