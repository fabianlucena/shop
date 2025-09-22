using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IPlanLimitService
        : IService<PlanLimit>,
            IServiceId<PlanLimit>,
            IServiceUuid<PlanLimit>,
            IServiceSoftDeleteUuid<PlanLimit>,
            IServiceName<PlanLimit>,
            IServiceIdUuidName<PlanLimit>
    {
    }
}
