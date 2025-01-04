using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using Microsoft.SqlServer.Types;

namespace backend_shop
{
    public class StoreAddRequest_CommerceIdResolverAsync(ICommerceService commerceService)
        : IValueResolver<StoreAddRequest, Store, Int64>
    {
        public Int64 Resolve(
            StoreAddRequest source,
            Store destination,
            Int64 destMember,
            ResolutionContext context)
        {
            return commerceService.GetSingleOrDefaultIdForUuidAsync(source.CommerceUuid)?.Result
                ?? throw new CommerceDoesNotExistException();
        }
    }

    public class ItemAddRequest_CategoryIdResolverAsync(ICategoryService categoryService)
        : IValueResolver<ItemAddRequest, Item, Int64>
    {
        public Int64 Resolve(
            ItemAddRequest source,
            Item destination,
            Int64 destMember,
            ResolutionContext context)
        {
            return categoryService.GetSingleOrDefaultIdForUuidAsync(source.CategoryUuid)?.Result
                ?? throw new CategoryDoesNotExistException();
        }
    }

    public class ItemAddRequest_StoreIdResolverAsync(IStoreService storeService)
        : IValueResolver<ItemAddRequest, Item, Int64>
    {
        public Int64 Resolve(
            ItemAddRequest source,
            Item destination,
            Int64 destMember,
            ResolutionContext context)
        {
            return storeService.GetSingleOrDefaultIdForUuidAsync(source.StoreUuid)?.Result
                ?? throw new StoreDoesNotExistException();
        }
    }

    public class MappingProfile
        : Profile
    {
        public MappingProfile()
        {
            CreateMap<CommerceAddRequest, Commerce>();
            CreateMap<Commerce, CommerceResponse>();

            CreateMap<StoreAddRequest, Store>()
                .ForMember(dest => dest.CommerceId, opt => opt.MapFrom<StoreAddRequest_CommerceIdResolverAsync>())
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location == null? null: SqlGeography.Point(src.Location.Lat, src.Location.Lng, 4326)));
            CreateMap<Store, StoreResponse>();

            CreateMap<Category, CategoryResponse>();

            CreateMap<ItemAddRequest, Item>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom<ItemAddRequest_CategoryIdResolverAsync>())
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom<ItemAddRequest_StoreIdResolverAsync>());
            CreateMap<Item, ItemResponse>();
        }
    }
}
