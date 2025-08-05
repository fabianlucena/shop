using backend_shop.Entities;
using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
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

        Task<QueryOptions> GetFilterForCurrentUserAsync(QueryOptions? options = null);

        Task<Int64> GetCountForCurrentUserAsync(QueryOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(QueryOptions? options = null);
    }
}
