using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IPlanService
        : IService<Plan>,
            IServiceId<Plan>,
            IServiceUuid<Plan>,
            IServiceSoftDeleteUuid<Plan>,
            IServiceName<Plan>,
            IServiceIdName<Plan>
    {
    }
}
