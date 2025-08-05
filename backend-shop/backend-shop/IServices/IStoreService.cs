using backend_shop.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
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

        Task<QueryOptions> GetFilterForCurrentUserAsync(QueryOptions? options = null);
        
        Task<Int64> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null);
    }
}
