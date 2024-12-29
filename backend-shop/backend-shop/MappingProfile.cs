using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;

namespace backend_shop
{
    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<CompanyAddRequest, Company>();
            CreateMap<Company, CompanyResponse>();
        }
    }
}
