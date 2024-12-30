using backend_shop.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shop.IServices
{
    public interface IPlanService
        : IService<Plan>,
            IServiceId<Plan>,
            IServiceUuid<Plan>,
            IServiceSoftDeleteUuid<Plan>,
            IServiceName<Plan>,
            IServiceIdUuidName<Plan>
    {
        Task<Plan> GetBaseAsync();

        Task<Plan> GetSingleOrBaseAsync(GetOptions options);
    }
}
