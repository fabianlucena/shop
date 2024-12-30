using backend_shop.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
{
    public interface IStoreService
        : IService<Store>,
            IServiceId<Store>,
            IServiceUuid<Store>,
            IServiceSoftDeleteUuid<Store>,
            IServiceName<Store>,
            IServiceIdUuidName<Store>
    {
        Task<bool> CheckForUuidAndCurrentUserAsync(Guid uuid, GetOptions? options = null);
    }
}
