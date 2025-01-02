using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using Microsoft.SqlServer.Types;

namespace backend_shop
{
    public class StoreAddRequest_BusinessIdResolverAsync(IBusinessService businessService)
        : IValueResolver<StoreAddRequest, Store, Int64>
    {
        public Int64 Resolve(
            StoreAddRequest source,
            Store destination,
            Int64 destMember,
            ResolutionContext context)
        {
            return businessService.GetSingleIdForUuidAsync(source.BusinessUuid).Result;
        }
    }

    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<BusinessAddRequest, Business>();
            CreateMap<Business, BusinessResponse>();

            CreateMap<StoreAddRequest, Store>()
                .ForMember(dest => dest.BusinessId, opt => opt.MapFrom<StoreAddRequest_BusinessIdResolverAsync>())
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location == null? null: SqlGeography.Point(src.Location.Lat, src.Location.Lng, 4326)));
            CreateMap<Store, StoreResponse>();
        }
    }
}
