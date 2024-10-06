using backend_buygi.Entities;
using backend_buygi.DTO;
using AutoMapper;

namespace backend_buygi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponse>();
        }
    }
}
