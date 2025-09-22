using backend_shopia.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shopia.IServices
{
    public interface IStoreService
        : IService<Store>,
            IServiceId<Store>,
            IServiceUuid<Store>,
            IServiceIdUuid<Store>,
            IServiceSoftDeleteUuid<Store>,
            IServiceName<Store>,
            IServiceIdUuidName<Store>
    {
        Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null);

        Task<QueryOptions> GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null);
    }
}
