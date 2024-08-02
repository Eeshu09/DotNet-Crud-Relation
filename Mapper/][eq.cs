using AutoMapper;
using Crud.Model;

namespace Crud.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
