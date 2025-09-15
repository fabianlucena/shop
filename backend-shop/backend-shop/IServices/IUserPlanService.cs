using backend_shop.DTO;
using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
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
