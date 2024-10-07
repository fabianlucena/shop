using AutoMapper;
using RFAuth.Entities;
using RFRegister.DTO;

namespace RFRegister
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterData, User> ();
        }
    }
}
