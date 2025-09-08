using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;

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
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location == null? null: src.Location.ToGeometryPoint()));
            CreateMap<Store, StoreResponse>();
            CreateMap<Store, StoreMinimalDTO>();

            CreateMap<Category, CategoryResponse>();
            CreateMap<Category, CategoryMinimalDTO>();

            CreateMap<ItemAddRequest, Item>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom<ItemAddRequest_CategoryIdResolverAsync>())
                .ForMember(dest => dest.StoreId, opt => opt.MapFrom<ItemAddRequest_StoreIdResolverAsync>());
            CreateMap<Item, ItemResponse>()
                .ForMember(dest => dest.CategoryUuid, opt => opt.MapFrom(src => src.Category != null ? (Guid?)src.Category.Uuid : null))
                .ForMember(dest => dest.StoreUuid, opt => opt.MapFrom(src => src.Store != null ? (Guid?)src.Store.Uuid : null));

            CreateMap<Plan, PlanResponse>();
        }
    }
}
