using backend_shopia.DTO;
using backend_shopia.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shopia.IServices
{
    public interface IItemService
        : IService<Item>,
            IServiceId<Item>,
            IServiceUuid<Item>,
            IServiceIdUuid<Item>,
            IServiceSoftDeleteUuid<Item>,
            IServiceName<Item>,
            IServiceIdUuidName<Item>
    {
        Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null);

        Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null);

        Task<IEnumerable<Guid>> GetListUuidForCurrentUserAsync(QueryOptions? options = null);

        Task<int> UpdateInheritedForUuid(Guid uuid, QueryOptions? options = null);

        Task<int> UpdateInheritedForStoreUuid(Guid storeUuid, QueryOptions? options = null);

        Task<int> UpdateInheritedForCommerceUuid(Guid commerceUuid, QueryOptions? options = null);
    }
}
