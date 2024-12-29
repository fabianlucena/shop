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
            CreateMap<BusinessAddRequest, Business>();
            CreateMap<Business, BusinessResponse>();
        }
    }
}
