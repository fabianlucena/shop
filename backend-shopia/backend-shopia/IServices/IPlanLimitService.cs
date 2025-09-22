using backend_shopia.Entities;
using RFService.IServices;

namespace backend_shopia.IServices
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
