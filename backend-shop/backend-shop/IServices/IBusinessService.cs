using backend_shop.Entities;
using Microsoft.AspNetCore.Http;
using RFAuth.Exceptions;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
{
    public interface IBusinessService
        : IService<Business>,
            IServiceId<Business>,
            IServiceUuid<Business>,
            IServiceIdUuid<Business>,
            IServiceSoftDeleteUuid<Business>,
            IServiceName<Business>,
            IServiceIdUuidName<Business>
    {
        Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null);

        Task<GetOptions> GetFilterForCurrentUserAsync(GetOptions? options = null);

        Task<Int64> GetCountForCurrentUserAsync(GetOptions? options = null);

        Task<IEnumerable<Int64>> GetListIdForCurrentUserAsync(GetOptions? options = null);
    }
}
