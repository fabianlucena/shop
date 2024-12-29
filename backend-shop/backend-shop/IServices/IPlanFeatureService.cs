using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IPlanFeatureService
        : IService<PlanFeature>,
            IServiceId<PlanFeature>,
            IServiceUuid<PlanFeature>,
            IServiceSoftDeleteUuid<PlanFeature>,
            IServiceName<PlanFeature>
    {
    }
}
