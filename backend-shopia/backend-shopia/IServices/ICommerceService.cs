using backend_shopia.Entities;
using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;
using RFService.IServices;
using RFService.Repo;

namespace backend_shopia.IServices
{
    public interface ICommerceService
        : IService<Commerce>,
            IServiceId<Commerce>,
            IServiceUuid<Commerce>,
            IServiceIdUuid<Commerce>,
            IServiceSoftDeleteUuid<Commerce>,
            IServiceName<Commerce>,
            IServiceIdUuidName<Commerce>
    {
        Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, QueryOptions? options = null);

        QueryOptions GetFilterForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<int> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForOwnerIdAsync(Int64 ownerId, QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null);
    }
}
