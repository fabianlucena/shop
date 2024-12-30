using backend_shop.Entities;
using RFService.IServices;

namespace backend_shop.IServices
{
    public interface IUserPlanService
        : IService<UserPlan>,
            IServiceTimestamps<UserPlan>
    {
        Task<int> GetMaxEnabledBusinessForUserId(Int64 userId);

        Task<int> GetMaxEnabledBusinessForCurrentUser();

        Task<int> GetMaxEnabledStoresForCurrentUser();
    }
}
