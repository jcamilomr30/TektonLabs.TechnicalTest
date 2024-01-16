using AutoMapper;
using TektonLabs.TechnicalTest.Core.Dtos;
using TektonLabs.TechnicalTest.Domain.Entities;

namespace TektonLabs.TechnicalTest.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductDto>()
                .ReverseMap();

            CreateMap<CreateProductDto, Product>()
                .ForMember(x => x.Id, c => c.MapFrom(src => src.ProductId));

            CreateMap<UpdateProductDto, Product>();

        }
    }
}
