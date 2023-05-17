using AutoMapper;
using CustomerDemo.Dto;
using CustomerDemo.Model;

namespace CustomerDemo
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CustomerCreateDto, Customer>()
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Surename, opt => opt.MapFrom(src => src.Surename))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<CustomerListDto, Customer>()
                .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
                .ForMember(dest => dest.Surename, opt => opt.MapFrom(src => src.Surename))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
