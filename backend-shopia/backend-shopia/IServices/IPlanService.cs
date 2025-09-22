using backend_shopia.DTO;
using backend_shopia.Entities;
using RFService.IServices;
using RFService.Repo;

namespace backend_shopia.IServices
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

        Task<Plan> GetSingleOrBaseAsync(QueryOptions options);

        Task<PlanLimits> GetLimitsForPlanAsync(Plan plan, QueryOptions? options = null);
    }
}
