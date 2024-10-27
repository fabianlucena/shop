using AutoMapper;
using RFAuth.Entities;
using RFRegister.DTO;

namespace RFRegister
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterData, User> ()
                .ForMember(dest => dest.IsEnabled, act => act.MapFrom(src => true)); // Set IsEnabled = true
        }
    }
}
