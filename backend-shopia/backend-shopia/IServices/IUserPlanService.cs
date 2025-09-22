using backend_shopia.DTO;
using backend_shopia.Entities;
using RFService.IServices;

namespace backend_shopia.IServices
{
    public interface IUserPlanService
        : IService<UserPlan>,
            IServiceTimestamps<UserPlan>
    {
        Task<Plan> GetSinglePlanForCurrentUserAsync();

        Task<UsedPlanDTO> GetUsedPlanForCurrentUserAsync();

        Task<PlanLimits> GetLimitsForCurrentUserAsync();
    }
}
